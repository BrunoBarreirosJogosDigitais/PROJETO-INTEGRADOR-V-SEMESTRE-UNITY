using UnityEngine;

public class MusicaPersistente : MonoBehaviour
{
    private static MusicaPersistente instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
