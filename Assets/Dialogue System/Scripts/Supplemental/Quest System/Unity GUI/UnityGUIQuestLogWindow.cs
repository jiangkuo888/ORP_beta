using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.UnityGUI {
	
	/// <summary>
	/// This is a Unity GUI implementation of QuestLogWindow. Don't confuse it with the
	/// deprecated UnityQuestLogWindow, which was the old quest log window implementation.
	/// </summary>
	public class UnityGUIQuestLogWindow : QuestLogWindow {

		/// <summary>
		/// The GUI root.
		/// </summary>
		public GUIRoot guiRoot;
		
		/// <summary>
		/// The scroll view where quest titles and descriptions will be shown.
		/// </summary>
		public GUIScrollView scrollView;
		
		/// <summary>
		/// The button to show active quests. When clicked, it should send the message
		/// "OnShowActiveQuests" to the UnityQuestLogWindow.
		/// </summary>
		public GUIButton activeButton;
		
		/// <summary>
		/// The button to show completed quests. When clicked, it should send the message
		/// "OnShowCompletedQuests" to the UnityQuestLogWindow.
		/// </summary>
		public GUIButton completedButton;

		[Serializable]
		public class AbandonControls {
			public GUIControl panel;
			public GUILabel questTitleLabel;
			public GUIButton ok;
			public GUIButton cancel;
		}

		public AbandonControls abandonQuestPopup = new AbandonControls();
		
		/// <summary>
		/// The name of the GUI style to use for quest titles. If blank, defaults to button.
		/// </summary>
		public string questHeadingGuiStyleName;
		
		/// <summary>
		/// The name of the GUI style to use for quest descriptions. If blank, defaults to label.
		/// </summary>
		public string questBodyGuiStyleName;
		
		/// <summary>
		/// The name of the GUI style to use for active quest entries. If blank, defaults to label.
		/// </summary>
		public string questEntryActiveGuiStyleName;
		
		/// <summary>
		/// The name of the GUI style to use when displaying successfully-completed entries. 
		/// If blank, it defaults to label.
		/// </summary>
		public string questEntrySuccessGuiStyleName;
		
		/// <summary>
		/// The name of the GUI style to use when displaying failed entries. 
		/// If blank, it defaults to label.
		/// </summary>
		public string questEntryFailureGuiStyleName;

		/// <summary>
		/// The name of the GUI style to use for Track and Abandon buttons.
		/// </summary>
		public string questEntryButtonStyleName;
		
		/// <summary>
		/// The name of the GUI style to use for the "No Active Quests" and "No Completed Quests"
		/// messages. If blank, defaults to label.
		/// </summary>
		public string noQuestsGuiStyleName;
		
		/// <summary>
		/// The message to show in the scrollView when there are no active quests.
		/// </summary>
		public string noActiveQuestsMessage = "No Active Quests";
		
		/// <summary>
		/// The message to show in the ScrollView when there are no completed quests.
		/// </summary>
		public string noCompletedQuestsMessage = "No Completed Quests";

		public int padding = 2;

		public string lastTooltip = " ";

		private GUIStyle questHeadingStyle = null;
		private GUIStyle questBodyStyle = null;
		private GUIStyle questEntryActiveStyle = null;
		private GUIStyle questEntrySuccessStyle = null;
		private GUIStyle questEntryFailureStyle = null;
		private GUIStyle questButtonStyle = null;

		private Action confirmAbandonQuestHandler = null;

		/// <summary>
		/// Attempts to find the GUI root and scroll view if they weren't set.
		/// Sets up the scroll view.
		/// </summary>
		public override void Awake() {
			base.Awake();
			if (guiRoot == null) guiRoot = GetComponentInChildren<GUIRoot>();
			if (scrollView == null) scrollView = GetComponentInChildren<GUIScrollView>();
			if (scrollView != null) {
				scrollView.MeasureContentHandler += OnMeasureContent;
				scrollView.DrawContentHandler += OnDrawContent;
			}
		}
		
		/// <summary>
		/// Start this instance by hiding the GUI root. We only need to activate it when the window
		/// is open.
		/// </summary>
		public void Start() {
			if (guiRoot != null) guiRoot.gameObject.SetActive(false);
		}
		
		public override void OpenWindow(Action openedWindowHandler) {
			if (guiRoot != null) {
				guiRoot.gameObject.SetActive(true);

				guiRoot.ManualRefresh();
			}
			openedWindowHandler();
		}
		
		public override void CloseWindow(Action closedWindowHandler) {
			if (guiRoot != null) guiRoot.gameObject.SetActive(false);
			closedWindowHandler();
		}
		
		public override void ConfirmAbandonQuest(string title, Action confirmAbandonQuestHandler) {
			this.confirmAbandonQuestHandler = confirmAbandonQuestHandler;
			OpenAbandonQuestPopup(title);
		}

		private void OpenAbandonQuestPopup(string title) {
			if (abandonQuestPopup.panel == null) {
				if (confirmAbandonQuestHandler != null) confirmAbandonQuestHandler();
			} else {
				if (abandonQuestPopup.questTitleLabel != null) abandonQuestPopup.questTitleLabel.text = title;
				abandonQuestPopup.panel.gameObject.SetActive(true);
			}
		}

		private void CloseAbandonQuestPopup() {
			if (abandonQuestPopup.panel != null) abandonQuestPopup.panel.gameObject.SetActive(false);
		}

		public void ClickConfirmAbandonQuest(object data) {
			CloseAbandonQuestPopup();
			if (confirmAbandonQuestHandler != null) confirmAbandonQuestHandler();
		}

		public void ClickCancelAbandonQuest(object data) {
			CloseAbandonQuestPopup();
		}

		/// <summary>
		/// The event handler that measures the size of the content that will go into the scroll
		/// view.
		/// </summary>
		public void OnMeasureContent() {
			MeasureQuestContent();
		}
		
		/// <summary>
		/// The event handler that draws the content of the scroll view.
		/// </summary>
		public void OnDrawContent() {
			DrawQuests();
		}

		private void MeasureQuestContent() {
			float contentHeight = padding;
			foreach (var questInfo in Quests) {
				contentHeight += QuestHeadingHeight(questInfo);
				if (IsSelectedQuest(questInfo)) {
					contentHeight += QuestDescriptionHeight(questInfo);
					contentHeight += QuestEntriesHeight(questInfo);
					contentHeight += QuestButtonsHeight(questInfo);
				}
			}
			contentHeight += padding;
			if (scrollView != null) scrollView.contentHeight = contentHeight;
		}

		public override void OnQuestListUpdated() {
			activeButton.clickable = !IsShowingActiveQuests;
			completedButton.clickable = IsShowingActiveQuests;
		}
		
		private GUIStyle UseGUIStyle(GUIStyle guiStyle, string guiStyleName, GUIStyle defaultStyle) {
			return (guiStyle != null) ? guiStyle : UnityGUITools.GetGUIStyle(guiStyleName, defaultStyle);
		}
		
		/// <summary>
		/// Computes the height of a quest heading, which is the height of the text or the height 
		/// of the active button, whichever is greater. This way headings will look similar to 
		/// the active/completed buttons (and will be tall enough if the skin has large buttons).
		/// </summary>
		/// <returns>
		/// The heading height.
		/// </returns>
		/// <param name='heading'>
		/// Heading.
		/// </param>
		private float QuestHeadingHeight(QuestInfo questInfo) {
			questHeadingStyle = UseGUIStyle(questHeadingStyle, questHeadingGuiStyleName, GUI.skin.button);
			return Mathf.Max(activeButton.rect.height, questHeadingStyle.CalcHeight(new GUIContent(questInfo.Heading.text), scrollView.contentWidth - (2 * padding)));
		}
		
		private float QuestDescriptionHeight(QuestInfo questInfo) {
			questBodyStyle = UseGUIStyle(questBodyStyle, questBodyGuiStyleName, GUI.skin.label);
			if (questHeadingSource == QuestHeadingSource.Name) {
				return questBodyStyle.CalcHeight(new GUIContent(questInfo.Description.text), scrollView.contentWidth - (2 * padding));
			} else {
				return 0;
			}
		}
		
		private float QuestEntriesHeight(QuestInfo questInfo) {
			float height = 0;
			for (int i = 0; i < questInfo.Entries.Length; i++) {
				QuestState entryState = questInfo.EntryStates[i];
				GUIStyle questEntryStyle = GetQuestEntryStyle(entryState);
				if (entryState != QuestState.Unassigned) {
					string entryDescription = questInfo.Entries[i].text;
					height += questEntryStyle.CalcHeight(new GUIContent(entryDescription), scrollView.contentWidth - (2 * padding));
				}
			}
			return height;
		}

		private float QuestButtonsHeight(QuestInfo questInfo) {
			if (questInfo.Trackable || questInfo.Abandonable) {
				questButtonStyle = UseGUIStyle(questButtonStyle, questEntryButtonStyleName, GUI.skin.button);
				questButtonStyle.wordWrap = false;
				return questButtonStyle.CalcHeight(new GUIContent("Abandon"), scrollView.contentWidth - (2 * padding));
			} else {
				return 0;
			}
		}
		
		private GUIStyle GetQuestEntryStyle(QuestState entryState) {
			questEntryActiveStyle = UseGUIStyle(questEntryActiveStyle, questEntryActiveGuiStyleName, GUI.skin.label);
			questEntrySuccessStyle = UseGUIStyle(questEntrySuccessStyle, questEntrySuccessGuiStyleName, GUI.skin.label);
			questEntryFailureStyle = UseGUIStyle(questEntryFailureStyle, questEntryFailureGuiStyleName, GUI.skin.label);
			switch (entryState) {
			case QuestState.Success: return questEntrySuccessStyle;
			case QuestState.Failure: return questEntryFailureStyle;
			default: return questEntryActiveStyle;
			}
		}
		
		private void DrawQuests() {





			if ((scrollView != null)) {
				



				float contentY = padding;
				foreach (var questInfo in Quests) {
					float headingHeight = QuestHeadingHeight(questInfo);


					if (GUI.Button(new Rect(padding, contentY, scrollView.contentWidth - (2 * padding), headingHeight), questInfo.Heading.text, questHeadingStyle)) {
						Rect newrect = new Rect(padding, contentY, scrollView.contentWidth - (2 * padding), headingHeight);

							if(GameObject.Find(PhotonNetwork.playerName))
							{
								GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = false;
								GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().OnGUI = true;	
							}
						else
							if(GameObject.Find(GameObject.Find ("GameManager").GetComponent<GameManagerVik>().characterName))
						{
							GameObject.Find(GameObject.Find ("GameManager").GetComponent<GameManagerVik>().characterName).GetComponent<DetectObjects>().enabled = false;
							GameObject.Find(GameObject.Find ("GameManager").GetComponent<GameManagerVik>().characterName).GetComponent<ClickMove>().OnGUI = true;	
						}

						ClickQuest(questInfo.Title);

						StartCoroutine(WaitMouseOut(0.3f));
					}




					contentY += headingHeight;
					if (IsSelectedQuest(questInfo)) {
						contentY = DrawQuestDescription(questInfo, contentY);
						contentY = DrawQuestEntries(questInfo, contentY);
						contentY = DrawQuestButtons(questInfo, contentY);
					}
				}
				if (Quests.Length == 0) {
					GUIStyle noQuestsStyle = UnityGUITools.GetGUIStyle(noQuestsGuiStyleName, GUI.skin.label);
					float descriptionHeight = noQuestsStyle.CalcHeight(new GUIContent(NoQuestsMessage), scrollView.contentWidth - 4);
					GUI.Label(new Rect(2, contentY, scrollView.contentWidth, descriptionHeight), NoQuestsMessage, noQuestsStyle);
					contentY += descriptionHeight;
				}


				



			}
		}
		
		private float DrawQuestDescription(QuestInfo questInfo, float contentY) {
			if (questHeadingSource == QuestHeadingSource.Name) {
				questBodyStyle = UseGUIStyle(questBodyStyle, questBodyGuiStyleName, GUI.skin.label);
				float descriptionHeight = questBodyStyle.CalcHeight(new GUIContent(questInfo.Description.text), scrollView.contentWidth - (2 * padding));
				GUI.Label(new Rect(padding, contentY, scrollView.contentWidth, descriptionHeight), questInfo.Description.text, questBodyStyle);
				return contentY + descriptionHeight;
			} else {
				return contentY;
			}
		}
		
		private float DrawQuestEntries(QuestInfo questInfo, float contentY) {
			float currentY = contentY;
			for (int i = 0; i < questInfo.Entries.Length; i++) {
				QuestState entryState = questInfo.EntryStates[i];
				if (entryState != QuestState.Unassigned) {
					string entryDescription = questInfo.Entries[i].text;
					GUIStyle questEntryStyle = GetQuestEntryStyle(entryState);
					float entryHeight = questEntryStyle.CalcHeight(new GUIContent(entryDescription), scrollView.contentWidth - (2 * padding));
					GUI.Label(new Rect(padding, currentY, scrollView.contentWidth, entryHeight), entryDescription, questEntryStyle);
					currentY += entryHeight;
				}
			}
			return currentY;
		}

		private float DrawQuestButtons(QuestInfo questInfo, float contentY) {
			float currentY = contentY;
			if ((currentQuestStateMask == QuestState.Active) && (questInfo.Trackable || questInfo.Abandonable)) {
				questButtonStyle = UseGUIStyle(questButtonStyle, questEntryButtonStyleName, GUI.skin.button);
				questButtonStyle.wordWrap = false;
				string trackText = GetLocalizedText("Track");
				Vector2 trackSize = questButtonStyle.CalcSize(new GUIContent(trackText)); 
				float buttonHeight = trackSize.y;
				float trackWidth = trackSize.x;
				string abandonText = GetLocalizedText("Abandon");
				float abandonWidth = questButtonStyle.CalcSize(new GUIContent(abandonText)).x;
				float rightX = scrollView.contentWidth - (2 * padding);
				float abandonX = rightX - abandonWidth;
				float trackX = questInfo.Abandonable ? (abandonX - padding) : rightX;
				trackX -= trackWidth;
				if (questInfo.Trackable) {
					if (GUI.Button(new Rect(trackX, currentY, trackWidth, buttonHeight), trackText)) {




						ClickTrackQuest(questInfo.Title);
					}
				}
				if (questInfo.Abandonable) {
					if (GUI.Button(new Rect(abandonX, currentY, abandonWidth, buttonHeight), abandonText)) {
						ClickAbandonQuest(questInfo.Title);
					}
				}
				currentY += questButtonStyle.CalcHeight(new GUIContent("Abandon"), scrollView.contentWidth - (2 * padding));
			}
			return currentY;
		}

		IEnumerator WaitMouseOut(float sec){
			
			
			Debug.Log("Quit lost focus");
			
			yield return new WaitForSeconds (sec);
			
			if(GameObject.Find(PhotonNetwork.playerName))
			{
				GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = true;
				GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().OnGUI = false;	
			}
			else
				if(GameObject.Find(GameObject.Find ("GameManager").GetComponent<GameManagerVik>().characterName))
			{
				GameObject.Find(GameObject.Find ("GameManager").GetComponent<GameManagerVik>().characterName).GetComponent<DetectObjects>().enabled = true;
				GameObject.Find(GameObject.Find ("GameManager").GetComponent<GameManagerVik>().characterName).GetComponent<ClickMove>().OnGUI = false;	
			}
			
		}
	


	}

}
