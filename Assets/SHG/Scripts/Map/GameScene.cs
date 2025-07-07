using UnityEngine;
using EditorAttributes;
using UnityEngine.SceneManagement;

namespace SHG
{
  [CreateAssetMenu (menuName = "ScriptableObjects/Map/Scene")]
  public class GameScene: ScriptableObject
  {
    [SerializeField, FilePath(filters: "unity")] [Validate("Scene must in build settings", nameof(IsInvalidScene), MessageMode.Error, buildKiller: true)]
    public string FileName;
    [SerializeField]
    public string Name;
    [SerializeField]
    public string KorName;

    protected bool IsInvalidScene()
    {
      if (this.FileName == null ||
        this.FileName.Replace(" ", "").Length == 0) {
        return (true);
      }
#if UNITY_EDITOR
      var index = SceneUtility.GetBuildIndexByScenePath(this.FileName);
      if (index == -1) {
        return (true);
      }
#endif
      return (false);
    }
  }
}
