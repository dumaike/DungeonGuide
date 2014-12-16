using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class PlayerCharacterRoot : CharacterRoot
    {
        private TileRoot[] allTiles;

        private MeshRenderer[] meshRenderers;
        
        private const float VISION_DISTANCE = 20.0f;
        private Vector3 eyeLevel = new Vector3(0, 0.1f, 0);
        
        private Color selectedColor = new Color(0, 1, 1, 1);
        private Color defaultColor = new Color(1, 1, 1, 1);

		#region initializers
		private void Awake()
		{
            this.allTiles = GameObject.FindObjectsOfType<TileRoot>();
            this.meshRenderers = this.GetComponentsInChildren<MeshRenderer> ();
		}

		private void Start()
		{

		}

		private void OnDestroy()
		{

		}
		#endregion

		#region public methods

		#endregion

		#region private methods
        private void Update()
        {
            Vector3 characterEyeLevel = this.transform.position + eyeLevel;
            foreach (TileRoot curTile in this.allTiles)
            {
                Ray raycastRay = new Ray(characterEyeLevel, curTile.transform.position - characterEyeLevel);
                RaycastHit hitInfo = new RaycastHit();
                Physics.Raycast(raycastRay, out hitInfo, VISION_DISTANCE);
                float distanceFromHitToTile = Vector3.Distance(hitInfo.point, curTile.transform.position);
                bool showTile = distanceFromHitToTile < 0.45;
                curTile.ShowTile(showTile);                   

                Debug.DrawLine(raycastRay.origin, hitInfo.point);
            }
        }

        public void CharacterSelected(bool selected)
        {
            foreach (MeshRenderer curRenderer in this.meshRenderers)
            {
                curRenderer.material.color = selected ? this.selectedColor : this.defaultColor;
            }
        }
		#endregion
	}

}

