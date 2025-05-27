using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public string foodName;
    public float eatTime = 3f;
    public float points = 10f;
    public int fullness = 15;
    public bool isGrabbed = false;
    public GameObject isTargetedBy = null;
}
