namespace UIScripts
{
    public static class UIController
    {
        public static LevelLoadPanel levelLoadPanel;
        public static UIPanel uIPanel;
        public static bool isStart = true;

        public static void ShowUIPanel() => uIPanel.gameObject.SetActive(true);

    }
}