using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuysoInputManager{

    public class MapInputManager : MonoBehaviour
    {
        [SerializeField]
        private Camera sceneCamera;

        private Vector3 lastPosition;
        
        [SerializeField]
        private LayerMask placementLayermask;

        public Vector3 getSelectedMapPosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = sceneCamera.nearClipPlane;
            Ray ray = sceneCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100, placementLayermask))
            {
                lastPosition = hit.point;
            }
            return lastPosition;
        }
    }
}

