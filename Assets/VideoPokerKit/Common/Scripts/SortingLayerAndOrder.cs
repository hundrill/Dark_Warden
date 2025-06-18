using UnityEngine;

namespace VideoPokerKit
{
    public class SortingLayerAndOrder : MonoBehaviour
    {
        public string sortingLayerName;
        public int sortingOrderInLayer;

        void Start()
        {
            // get renderer for current object
            Renderer rend = gameObject.GetComponent<Renderer>();
            // set desired sorting order
            rend.sortingLayerName = sortingLayerName;
            rend.sortingOrder = sortingOrderInLayer;
        }
    }
}