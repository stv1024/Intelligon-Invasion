using UnityEditor;

/// <summary>
/// Tester
/// </summary>
public class GameTester : EditorWindow
{
    [MenuItem("Fairwood/Game Tester")]
    static void Init()
    {
        var window = GetWindow<GameTester>("游戏测试器", true);
    }

    void Update()
    {
        Repaint();
    }

    private static float _enemyTimeScale = 1;
    void OnGUI()
    {

    }
}