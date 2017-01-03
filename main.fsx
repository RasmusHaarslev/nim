#load "AsyncEventQueue.fsx"
#load "Ai.fsx"
#load "Gui.fsx"
#load "GameController.fsx"
open System
open System.Windows.Forms
open System.Drawing

open AsyncEventQueue
open Ai
open Gui
open GameController



// We can initialize the Gui state here.
//Gui.populateHeapPanel [222;23234;1111;23;5]

Async.StartImmediate (GameController.menu())
Application.Run(Gui.mainWindow);