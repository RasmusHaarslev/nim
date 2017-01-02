#load "AsyncEventQueue.fsx"
#load "Ai.fsx"
#load "Gui.fsx"
open System
open System.Windows.Forms
open System.Drawing

open AsyncEventQueue
open Ai
open Gui


<<<<<<< HEAD
[<EntryPoint>]
[<STAThread>]
let main argv =

    Ai.aiMove [3; 24; 9]

    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault false


    Application.Run(Gui.radiobuttonform);

    0
=======
Application.Run(Gui.mainWindow);

>>>>>>> 9ec2c836f074e5f17ba4ed64adc57ebbe6f6fc83
