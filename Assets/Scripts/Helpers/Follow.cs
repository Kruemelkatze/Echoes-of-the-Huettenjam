using System;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Helpers
{
    public class Follow : MonoBehaviour
    {
        [SerializeField] protected FollowMode followMode = FollowMode.Update;
        [SerializeField] protected Transform target;

        protected Transform _originalTarget;
        public Transform Target => target;

        private Transform _transientCenterTransform;

        protected void Start()
        {
            _originalTarget = target;
        }

        protected void Update()
        {
            if ((FollowMode.Update & followMode) > 0 && target)
                Move();
        }

        protected void FixedUpdate()
        {
            if ((FollowMode.FixedUpdate & followMode) > 0 && target)
                Move();
        }

        protected void LateUpdate()
        {
            if ((FollowMode.LateUpdate & followMode) > 0 && target)
                Move();
        }

        protected virtual void Move()
        {
            transform.position = target.position;
        }

        #region Overriding Targets

        public virtual Transform CenterBetweenCurrentAndOthers(params Transform[] otherTransforms)
        {
            if (otherTransforms == null || otherTransforms.Length == 0)
                return target;

            var transforms = new[] {target}.Concat(otherTransforms).ToArray();
            return CenterBetweenTransforms(transforms);
        }

        public virtual Transform CenterBetweenTransforms(params Transform[] transforms)
        {
            if (transforms == null || transforms.Length == 0)
                return target;

            if (!_transientCenterTransform)
            {
                _transientCenterTransform = new GameObject($"{name} - TransientCenterTarget").transform;
            }

            _transientCenterTransform.position = VectorExt.Average(transforms.Select(t => (Vector2) t.position));

            OverrideTarget(_transientCenterTransform);
            return _transientCenterTransform;
        }

        public virtual void OverrideTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public virtual Transform ResetToOriginalTarget()
        {
            if (_transientCenterTransform)
            {
                Destroy(_transientCenterTransform.gameObject);
            }

            return target = _originalTarget;
        }
        #endregion

        [Flags]
        public enum FollowMode
        {
            Update = 1 << 0,
            LateUpdate = 1 << 1,
            FixedUpdate = 1 << 2,
        }
    }

}