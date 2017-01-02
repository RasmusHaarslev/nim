#load "AsyncEventQueue.fsx"
#load "Ai.fsx"
#load "Gui.fsx"
open System
open System.Windows.Forms
open System.Drawing

open AsyncEventQueue
open Ai
open Gui

//let randomArr = [1;3;4;6;8]
//populateHeapPanel randomArr

Gui.populateHeapPanel [222;23234;1111;23;5]

Gui.clearHeapPanel

Application.Run(Gui.mainWindow);
