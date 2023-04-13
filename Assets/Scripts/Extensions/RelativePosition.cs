using System;
using UnityEngine;

namespace Extensions
{
    [Serializable]
    public struct RelativePosition
    {
        public Vector3 Relative;
        public Transform RelativeTo;

        private bool _wasRelative;


        public RelativePosition(Transform relativeTo, Vector3 position, bool abs = true)
        {
            RelativeTo = relativeTo;
            Relative = relativeTo && abs ? relativeTo.InverseTransformPoint(position) : position;
            _wasRelative = relativeTo;
        }

        public RelativePosition(Transform relativeTo, float x, float y, float z = 0) : this()
        {
            RelativeTo = relativeTo;
            Relative = new Vector3(x, y, z);
            _wasRelative = relativeTo;
        }

        public bool Valid => _wasRelative == (bool) RelativeTo;
        public bool IsRelative => RelativeTo;
        public Vector3 Position => RelativeTo ? RelativeTo.TransformPoint(Relative) : Relative;

        // TODO: Migrate to this scheme, it's cleaner
        public static RelativePosition FromRelativePosition(Transform relativeTo, Vector3 relative)
        {
            return new RelativePosition(relativeTo, relative, false);
        }

        public static RelativePosition FromAbsolutePosition(Transform relativeTo, Vector3 absolute)
        {
            return new RelativePosition(relativeTo, absolute, true);
        }
    }
}