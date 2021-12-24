#r "nuget: MathNet.Numerics, 4.15.0"
#r "nuget: MathNet.Numerics.FSharp, 4.15.0"

open System.Numerics
open MathNet.Numerics.LinearAlgebra


#load "../utils.fsx"

open utils

type Report = Vector<double> list

let rotations =
    let rotate times matrix rotationMatrix =
        { 0 .. times }
        |> Seq.fold (fun rotate _ -> rotate * matrix) rotationMatrix

    seq {
        let rotateX =
            matrix [ [ 1.0; 0.0; 0.0 ]
                     [ 0.0; 0.0; -1.0 ]
                     [ 0.0; 1.0; 0.0 ] ]

        let rotateY =
            matrix [ [ 0.0; 0.0; 1.0 ]
                     [ 0.0; 1.0; 0.0 ]
                     [ -1.0; 0.0; 0.0 ] ]

        let rotateZ =
            matrix [ [ 0.0; -1.0; 0.0 ]
                     [ 1.0; 0.0; 0.0 ]
                     [ 0.0; 0.0; 1.0 ] ]

        for x in 0 .. 3 do
            for y in 0 .. 3 do
                for z in 0 .. 3 do
                    DenseMatrix.identity<double> 3
                    |> rotate x rotateX
                    |> rotate y rotateY
                    |> rotate z rotateZ
    }
    |> List.ofSeq


let align (report1: Report) (report2: Report) =
    let rotations = rotations

    let mutable aligned = None

    for rotation in
        rotations
        |> List.map (fun rotation -> report2 |> List.map rotation.LeftMultiply) do
        for rotated in rotation do
            for report1Component in report1 do
                let r2t =
                    rotation
                    |> List.map (fun vec -> vec + (report1Component - rotated))

                let intersection = intersect r2t report1

                if intersection |> Seq.length > 4 then
                    aligned <- Some(r2t, (report1Component - rotated))

    aligned

let alignAll (reports: Report list) : Report list * Report =
    let rec alignAllInner (aligned: Report) (toAlign: Report list) (scanners: Report) =
        match toAlign with
        | [] -> (toAlign, scanners)
        | _ :: _ ->
            match align aligned toAlign.Head with
            | Some (newAligned, newToAlign) ->
                alignAllInner ((newAligned @ aligned) |> List.distinct) toAlign.Tail (scanners @ [ newToAlign ])
            | None -> alignAllInner aligned (toAlign.Tail @ [ toAlign.Head ]) scanners

    alignAllInner reports.Head reports.Tail List.empty<Vector<double>>

let parse (input: string) =
    let (_, scanners: Map<int, Report>) =
        input
        |> toRows
        |> List.fold
            (fun (index, scanners) line ->
                match line with
                | "" -> (index + 1, scanners)
                | line when not <| line.StartsWith "---" ->
                    let newReport =
                        line.Split ","
                        |> Array.map double
                        |> (fun array ->
                            vector [ array.[0]
                                     array.[1]
                                     array.[2] ])

                    let current = scanners.TryFind index

                    match current with
                    | Some report -> (index, scanners.Add(index, newReport :: report))
                    | None -> (index, scanners.Add(index, [ newReport ]))

                | _ -> (index, scanners))
            (0, Map.empty<int, Report>)

    scanners |> Map.toList |> List.map snd

let parse2 (input: string) =
    let mutable scanners = Map.empty<int, Report>

    let mutable c = 0

    input
    |> toRows
    |> List.iter
        (fun line ->
            match line with
            | "" -> c <- c + 1
            | line when not <| line.StartsWith "---" ->
                let newReport =
                    line.Split ","
                    |> Array.map double
                    |> (fun array ->
                        vector [ array.[0]
                                 array.[1]
                                 array.[2] ])

                let current = scanners.TryFind c

                match current with
                | Some report -> scanners <- scanners.Add(c, newReport :: report)
                | None -> scanners <- scanners.Add(c, [ newReport ])

            | _ -> ())

    scanners |> Map.toList |> List.map snd

let manhattanDistance (report: Report) =
    report
    |> List.allPairs report
    |> List.map (fun (x, y) -> (x - y).L1Norm())

let solve =
    file "input.txt"
    |> parse
    |> tap (fun x -> printfn "%A" x)
    |> alignAll
    |> tap (fun (aligned, _) -> printfn "Part 1: %A" aligned.Length)
    |> snd
    |> manhattanDistance
    |> tap (fun x -> printfn "%A" x)
    |> List.max
    |> tap (fun x -> printfn "Part 2: %A" x)
