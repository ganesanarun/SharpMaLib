﻿// * **********************************************************************************************
// * Copyright (c) Edmondo Pentangelo. 
// *
// * This source code is subject to terms and conditions of the Microsoft Public License. 
// * A copy of the license can be found in the License.html file at the root of this distribution. 
// * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
// * Microsoft Public License.
// *
// * You must not remove this notice, or any other, from this software.
// * **********************************************************************************************


module ListTests

open NUnit.Framework
open NUnitFsCheck
open Monad.List

let (>>=) m f = list.Bind (m, f)
let unit = list.Return

[<TestFixture>]
type ListTests =
    new() = {}
    
    [<Test>]
    member x.MonadLaws() =
        // Left unit  
        quickCheck (fun m f a -> ((unit a) >>= f) = f a) 
        // Right unit
        quickCheck (fun m  -> (m >>= unit) = m)
        // Associative
        quickCheck (fun m f g-> ((m >>= f) >>= g) = (m >>= (fun x -> f x >>= g)))
        
    [<Test>]
    member x.MonadLawsInTermsOfMapAndJoin() =
        let f x = x / 2
        let g x = x - 2
        quickCheck (fun x -> map id x = id x)
        quickCheck (fun x -> map (f << g) x = ((map f) << (map g)) x)
        quickCheck (fun x -> (map f << unit) x = (unit << f) x)
        quickCheck (fun x -> (map f << join) x = (join << (map (map f))) x)
        quickCheck (fun x -> (join << unit) x = id x)
        quickCheck (fun x -> (join << map unit) x = id x)
        quickCheck (fun x -> (join << map join) x = (join << join) x)
        quickCheck (fun m k -> m >>= k = join(map k m))
        