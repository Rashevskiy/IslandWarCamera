# IslandWarCamera
My own script for camera with controller from the island war game

Script cross platform
<source lang="cs">

    private void Start()
    {
        localCentrPos = transform.position + transform.forward * localRadius * (transform.position.y * 0.2f);

        #if UNITY_EDITOR
        input = new MouseCameraInput();
        #elif UNITY_IOS
        input = new TouchCameraInput();
        #elif UNITY_ANDROID
        input = new TouchCameraInput();
        #elif UNITY_STANDALONE_WIN
        input = new MouseCameraInput();
       #endif
    }
    
</source>
