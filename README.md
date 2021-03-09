[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ZsoltGajdacs_UsageWatcher&metric=alert_status)](https://sonarcloud.io/dashboard?id=ZsoltGajdacs_UsageWatcher)
[![Nuget](https://img.shields.io/nuget/v/ZsGWorks.UsageWatcher)](https://www.nuget.org/packages/ZsGWorks.UsageWatcher)
# UsageWatcher
A simple way to watch mouse and keyboard usage for tracking purposes

## Purpose
A central library for my own applications to watch for keyboard / mouse usage in order to track active computer use.

## Feedback
If you feel like this is useful, or could be useful for you, feel free to create a feature request. If you think something is not working well, please create a bug report. Thank you!

## How does it work
The idea is very simple: You pass a given 'resolution', like two minutes, and any mouse or keyboard movement within that timeframe will be considered an active time.
If the computer is locked usage is not counted.

## What it doesn't do
It's not logging anything. Neither software usage, nor which keys were pressed. You can check.

## Usage
There is a probe project included, this line is taken from there:
```
IWatcher watcher = new Watcher("testApp", Resolution.HalfMinute, SavePreference.KeepDataForToday, DataPrecision.HighPrecision);
```
Then you can just ask, also from the probe project:
~~~
TimeSpan usage = watcher.UsageForGivenTimeframe(startTime, startTime + TimeSpan.FromDays(1));
~~~
