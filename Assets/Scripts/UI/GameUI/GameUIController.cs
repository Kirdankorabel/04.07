using UIScripts;

public static class GameUIController
{
    public static WinPanel WinPanel;
    public static ExitPanel ExitPanel;
    public static void Win()
    {
        WinPanel.gameObject.SetActive(true);
    }
    public static void Exit()
    {
        ExitPanel.gameObject.SetActive(true);
    }
}
