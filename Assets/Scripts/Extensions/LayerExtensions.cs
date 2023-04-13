using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class LayerExtensions
    {
        public static bool Test(this LayerMask mask, int index)
        {
            return (mask & (1 << index)) != 0;
        }

        public static bool Test(this LayerMask mask, Component comp)
        {
            return Test(mask, comp.gameObject.layer);
        }

        public static int[] GetSortingLayerMask(params string[] sortingLayerNames)
        {
            return sortingLayerNames.Select(SortingLayer.NameToID).ToArray();
        }

        public static LayerMask GetLayerMask(params string[] layerNames)
        {
            return layerNames.Aggregate(0, (mask, layerName) => mask |= 1 << LayerMask.NameToLayer(layerName));
        }
    }
}