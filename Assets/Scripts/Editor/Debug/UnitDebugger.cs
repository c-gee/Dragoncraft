using UnityEditor;

namespace Dragoncraft
{
    public static class UnitDebugger
    {
        [MenuItem("Dragoncraft/Debug/Unit/Spawn Warrior %g")]
        private static void SpawnWarrior()
        {
            MessageQueueManager.Instance.SendMessage(new BasicWarriorSpawnMessage());
        }
    }
}