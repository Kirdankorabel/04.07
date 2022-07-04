using UnityEngine;

namespace KnotGameController
{
    public class Connection : MonoBehaviour
    {
        private int _number;
        public bool isFree;
        public int Number
        {
            get => _number;
            set
            {
                if (_number == 0)
                    _number = value;
            }
        }

        private void OnMouseDown()
        {
            if (isFree && GameController.ActiveConductor != null)
                GameController.ActiveConductor.Connect(this);
        }
    }
}
