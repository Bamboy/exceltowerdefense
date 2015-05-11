using UnityEngine;
using UnityEngine.UI;
using Excelsion.GameManagers;

// Matt McGrath - 4/25/2015
namespace Excelsion.UI
{
	// Resource Reader is a means to abstract and separate Resource Controller functionality with UI-related stuff.
	public class ResourceReader : MonoBehaviour 
	{
		// Reference to our Parent UI object that handles Resource UI.
		public GameObject selectedInfoToggle;

		// Reference to our ResourceController, so we can grab Resource information without calling .Get() each time.
		private ResourceController resourceController;

		// UI Text objects the Parent UI will contain.
		public Text populationText;
		public Text foodText;
		public Text woodText;
		public Text stoneText;
		public Text metalText;

		// UI Image objects the Parent UI will contain. Icons are more visually-friendly than writing "Food: ".
		public Image populationImage;
		public Image foodImage;
		public Image woodImage;
		public Image stoneImage;
		public Image metalImage;

		// On Start, we want to make the parent UI object active.
		void Start () 
		{
			// We always want to show our Resource list (I believe?)
			selectedInfoToggle.SetActive(true);

			// Grab our ResourceController instance.
			resourceController = ResourceController.Get();
		}
		
		void Update () 
		{
			// We don't want to see or update this UI of we're Paused.
			if (WorldClock.Pause)
			{
				selectedInfoToggle.SetActive(true);
				return;
			}

			selectedInfoToggle.SetActive(true);

			// Update the UI.
			UpdateResourceUI();
		}

		// Updates the Resource UI by filling in the values of our Resources. TODO: Use Icons instead of writing "Food", "Wood", etc?
		public void UpdateResourceUI()
		{
			populationText.text = "Population: " + resourceController.ResourceAmount(ResourceType.Population);
			foodText.text = "Food: " +  resourceController.ResourceAmount(ResourceType.Food);
			woodText.text = "Wood: " +  resourceController.ResourceAmount(ResourceType.Wood);
			stoneText.text = "Stone: " + resourceController.ResourceAmount(ResourceType.Stone);
			metalText.text = "Metal: " + resourceController.ResourceAmount(ResourceType.Metal);
		}

		public void HideShow()
		{
			// Slide the menu in? For now just activate or deactivate it.
			gameObject.SetActive(!gameObject.activeSelf);

			//displayButton.GetComponent<TabSlideOut>().isSliding = true;
		}
	}
}