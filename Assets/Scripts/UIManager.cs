using UnityEngine;
using UnityEngine.UI;

namespace ConveyorX.XR.UI
{
    public class UIManager : MonoBehaviour
    {
        public Image jetpackFill;

        public void UpdateUI(float currentFuel, float maxFuel)
        {
            jetpackFill.fillAmount = currentFuel / maxFuel;
        }
    }
}