# NetConMon - Network Connectivity Monitoring Service
Windows network connectivity monitor service

Logs network timeout and re-availability to the windows eventlog

Use it to monitor and debug your network problems

Maybe in a future update: GUI

## Installation
```installutil.exe NetConMon.exe```


## Deinstallation
```installutil.exe /u NetConMon.exe```


## Configuration
```Computer\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetConMon\Parameters```


## View log
```Eventlog\NetMonConLog```

