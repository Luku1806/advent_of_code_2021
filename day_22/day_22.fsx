#load "../utils.fsx"

open utils

open System.Text.RegularExpressions

type Cuboid =
    {
        xMin: int64
        xMax: int64
        yMin: int64
        yMax: int64
        zMin: int64
        zMax: int64
        on: bool
    }
    static member parse(line: string) =
        let matches =
            Regex.Match(
                line,
                "(?<state>(on|off)) x=(?<xMin>-?\d+)..(?<xMax>-?\d+),y=(?<yMin>-?\d+)..(?<yMax>-?\d+),z=(?<zMin>-?\d+)..(?<zMax>-?\d+)"
            )

        {
            on = matches.Groups.["state"].Value = "on"
            xMin = int matches.Groups.["xMin"].Value
            xMax = int matches.Groups.["xMax"].Value
            yMin = int matches.Groups.["yMin"].Value
            yMax = int matches.Groups.["yMax"].Value
            zMin = int matches.Groups.["zMin"].Value
            zMax = int matches.Groups.["zMax"].Value
        }

    member this.Size: int64 =
        (this.xMax - this.xMin + 1L)
        * (this.yMax - this.yMin + 1L)
        * (this.zMax - this.zMin + 1L)


    member this.Overlap (cuboid: Cuboid) (on: bool) =
        let xOverlap =
            Cuboid.OverlapDimension this.xMin this.xMax cuboid.xMin cuboid.xMax

        let yOverlap =
            Cuboid.OverlapDimension this.yMin this.yMax cuboid.yMin cuboid.yMax

        let zOverlap =
            Cuboid.OverlapDimension this.zMin this.zMax cuboid.zMin cuboid.zMax

        match xOverlap, yOverlap, zOverlap with
        | Some (xMin, xMax), Some (yMin, yMax), Some (zMin, zMax) ->
            Some
                {
                    on = on
                    xMin = xMin
                    xMax = xMax
                    yMin = yMin
                    yMax = yMax
                    zMin = zMin
                    zMax = zMax
                }
        | _ -> None

    static member OverlapDimension cuboid1Min cuboid1Max cuboid2Min cuboid2Max =
        if cuboid1Min <= cuboid2Max
           && cuboid1Max >= cuboid2Min then
            Some(max cuboid1Min cuboid2Min, min cuboid1Max cuboid2Max)
        else
            None

type Cuboids =
    {
        cuboids: Cuboid List
    }
    member this.Count50By50 =
        this.cuboids
        |> List.fold
            (fun all cuboid ->
                let xMin = max cuboid.xMin -50
                let xMax = min cuboid.xMax 50

                { xMin .. xMax }
                |> Seq.fold
                    (fun all x ->
                        let yMin = max cuboid.yMin -50
                        let yMax = min cuboid.yMax 50

                        { yMin .. yMax }
                        |> Seq.fold
                            (fun all y ->
                                let zMin = max cuboid.zMin -50
                                let zMax = min cuboid.zMax 50

                                { zMin .. zMax }
                                |> Seq.fold
                                    (fun all z ->
                                        if cuboid.on then
                                            all |> Set.add (x, y, z)
                                        else
                                            all |> Set.remove (x, y, z))
                                    all)
                            all)
                    all)
            Set.empty<int64 * int64 * int64>
        |> Set.count

    member this.Count =
        if this.cuboids.IsEmpty then
            0L
        else
            this.cuboids
            |> List.indexed
            |> List.fold
                (fun sum (index, cuboid) ->
                    let alreadyOn =
                        this.cuboids.[..index - 1]
                        |> List.map (fun current -> cuboid.Overlap current current.on)
                        |> List.filter (fun overlap -> overlap.IsSome)
                        |> List.map (fun cuboid -> cuboid.Value)
                        |> fun cuboids -> { cuboids = cuboids }
                        |> fun below -> below.Count

                    if cuboid.on then
                        sum + cuboid.Size - alreadyOn
                    else
                        sum - alreadyOn)
                0L

let solve () =
    file "input.txt"
    |> toRows
    |> List.map Cuboid.parse
    |> fun cuboids -> { cuboids = cuboids }
    |> tap (fun cuboids -> printfn "Part1: %A" cuboids.Count50By50)
    |> tap (fun cuboids -> printfn "Part2: %A" (cuboids.Count))

solve ()
