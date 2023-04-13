using Helpers;
using UnityEngine;

namespace Extensions
{
    public static class CameraExtensions
    {
        public static Bounds OrthographicBounds(this Camera camera)
        {
            var height = camera.orthographicSize * 2;
            var width = height * camera.aspect;

            Bounds bounds = new Bounds(
                camera.transform.position,
                new Vector3(width, height, 0));

            return bounds;
        }

        public static Transform CenterBetweenTransforms(params Transform[] objects)
        {
            var cameraFollower = Camera.main.GetComponent<Follow>();
            return !cameraFollower ? null : cameraFollower.CenterBetweenTransforms(objects);
        }

        public static Transform CenterBetweenCurrentAndOthers(params Transform[] objects)
        {
            var cameraFollower = Camera.main.GetComponent<Follow>();
            return !cameraFollower ? null : cameraFollower.CenterBetweenCurrentAndOthers(objects);
        }

        public static void ResetFocus()
        {
            var cameraFollower = Camera.main.GetComponent<Follow>();
            if (cameraFollower)
            {
                cameraFollower.ResetToOriginalTarget();
            }
        }
    }
}