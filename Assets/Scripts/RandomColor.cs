using UnityEngine;

public class RandomColor : MonoBehaviour
{
   [SerializeField] private MeshRenderer meshRenderer;

   private void Awake()
   {
      meshRenderer.material.color = Random.ColorHSV(
         0f, 1f, 1f, 1f, 0.5f, 1f
         );
         
         
         
   }
   
}
