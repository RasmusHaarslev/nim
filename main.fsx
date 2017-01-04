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

Async.StartImmediate (GameController.menu())
Application.Run(Gui.mainWindow);