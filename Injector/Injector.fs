module Injector
open System
open System.IO
open System.Diagnostics
open System.Runtime.Remoting
open EasyHook
open Injectable


let sourceDir = Directory.GetParent __SOURCE_DIRECTORY__ |> string
let buildDir = Path.Combine(sourceDir, "build")
let dllPath = Path.Combine(buildDir, "Injectable.dll")
let exePath = Path.Combine(buildDir, "HackMe.exe")

// A simple wrapper around IpcCreateServer that will return the assigned channel name
let createIpcServer<'Logger when 'Logger :> MarshalByRefObject>(objMode: WellKnownObjectMode) =
    let channelName =
        ref null
    let ipcServer =
        RemoteHooking.IpcCreateServer<'Logger>(channelName, objMode)
    ipcServer, !channelName

[<EntryPoint>]
let main argv =
    use p =
        Process.Start(exePath)

    let ipcLogChannel, channelName =
        createIpcServer<Logger> WellKnownObjectMode.Singleton

    printfn "[INJECTOR] Log channel ID < %s > " channelName

    RemoteHooking.Inject(
        p.Id,
        InjectionOptions.DoNotRequireStrongName,
        dllPath,
        dllPath,
        channelName
    )

    Console.ReadLine() |> ignore
    0
