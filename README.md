# NetConMon - Network Connectivity Monitoring Service
Logs network timeout and re-availability to the windows eventlog

Default: Ping 8.8.8.8 every 5s with 500ms timeout

Use it to monitor and debug your network problems.

Maybe in a future update: GUI!

## Installation
```installutil.exe NetConMon.exe```


## Deinstallation
```installutil.exe /u NetConMon.exe```


## Configuration
```Computer\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetConMon\Parameters```


## View log
```Eventlog\NetMonConLog```

