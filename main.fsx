#load "AsyncEventQueue.fsx"
#load "Gui.fsx"
open System
open System.Windows.Forms
open System.Drawing

open AsyncEventQueue
open Gui


[<EntryPoint>]
[<STAThread>]
let main argv = 
 
    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault false
 
 
    Application.Run(Gui.radiobuttonform);
 
    0 