using System;
using System.Collections;
using Extensions;
using UnityEngine;

namespace Helpers
{
    public class SmoothMove : Follow
    {
        [Space] [SerializeField] private bool moveToTargetOnStart = true;

        [Space] public float smoothTime = 0.2f;
        public bool freezeZ = true;
        [ReadOnlyField] public SmoothSpeedFactor smoothSpeedFactor;

        private Vector3 _velocity = Vector3.zero;

        protected new void Start()
        {
            base.Start();

            if (moveToTargetOnStart)
            {
                MoveInstant();
            }
        }

        protected override void Move()
        {
            if (!target)
                return;

            var p = target.position;
            var tp = transform.position;
            var targetPosition = freezeZ ? new Vector3(p.x, p.y, tp.z) : p;

            var effectiveSmoothTime = smoothSpeedFactor switch
            {
                SmoothSpeedFactor.Default => smoothTime,
                SmoothSpeedFactor.Slower => smoothTime * 2,
                SmoothSpeedFactor.Faster => smoothTime / 2,
                _ => throw new ArgumentOutOfRangeException()
            };
            transform.position = Vector3.SmoothDamp(tp, targetPosition, ref _velocity, effectiveSmoothTime);
        }

        protected void MoveInstant()
        {
            if (!target)
                return;

            var p = target.position;
            var tp = transform.position;
            var targetPosition = freezeZ ? new Vector3(p.x, p.y, tp.z) : p;

            transform.position = targetPosition;
        }

        public override void OverrideTarget(Transform newTarget)
        {
            // Assumption: When focussing on other element, we always want it to move slower.
            smoothSpeedFactor = SmoothSpeedFactor.Slower;
            base.OverrideTarget(newTarget);
        }

        public override Transform ResetToOriginalTarget()
        {
            if (smoothSpeedFactor != SmoothSpeedFactor.Default)
            {
                StartCoroutine(ReturnToDefaultSmoothSpeedFactor());
            }

            return base.ResetToOriginalTarget();
        }

        private IEnumerator ReturnToDefaultSmoothSpeedFactor()
        {
            yield return new WaitForSeconds(smoothTime);
            smoothSpeedFactor = SmoothSpeedFactor.Default;
        }

        public enum SmoothSpeedFactor
        {
            Default,
            Slower,
            Faster,
        }
    }
}