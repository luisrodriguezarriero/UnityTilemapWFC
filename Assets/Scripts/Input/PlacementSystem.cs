using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuysoInputManager
{
    public class MouseSystem : MonoBehaviour
    {
        [SerializeField] private GameObject mouseIndicator, cellIndicator;
        [SerializeField] private MapInputManager inputManager;
        [SerializeField] private Grid grid;
        [SerializeField] private Vector2 offset;
        private GameObject chosenCharacter;
        private void Update()
        {
            Vector3 mousePosition = inputManager.getSelectedMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);
            mouseIndicator.transform.position = mousePosition;
            cellIndicator.transform.position = grid.CellToWorld(gridPosition);
        }
    }
}
