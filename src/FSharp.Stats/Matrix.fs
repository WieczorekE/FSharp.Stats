//namespace Microsoft.FSharp.Math // old namespace
namespace FSharp.Stats

#nowarn "60" // implementations in augmentations
#nowarn "69" // implementations in augmentations

//open Microsoft.FSharp.Math
open System
open System.Globalization
open System.Collections
open System.Collections.Generic
open System.Diagnostics

//type matrix = Matrix<float>
//type vector = Vector<float>
//type rowvec = RowVector<float>

module MRandom = 
    let seed = 99
    let randomGen = new System.Random(seed)
    let float f = randomGen.NextDouble() * f 


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Matrix = begin
        
    module Generic = begin

        module MS = SpecializedGenericImpl

        // Accessors
        let get (a:Matrix<_>) i j   = a.[i,j]
        let set (a:Matrix<_>) i j x = a.[i,j] <- x
                  
        // Creation
        let ofList    xss      = MS.listM  xss
        let ofSeq     xss      = MS.seqM  xss
        let init  m n f       = MS.initM  m n f
        let ofArray2D (arr: 'T[,])  : Matrix<'T>       = MS.arrayM arr
        let toArray2D (m:Matrix<_>) = Array2D.init m.NumRows m.NumCols (fun i j -> get m i j)
        let initNumeric m n f = MS.initNumericM m n f
        let zero m n            = MS.zeroM m n
        let identity m          = MS.identityM m
        let create  m n x       = MS.constM m n x

        let ofScalar   x        = MS.scalarM x

        let diag v              = MS.diagM v
        let initDiagonal v      = MS.diagM v
        let constDiag   n x     = MS.constDiagM n x
          
        // Operators
        let add a b = MS.addM a b
        let sub a b = MS.subM a b
        let mul a b = MS.mulM a b
        let mulRV a b = MS.mulRVM a b
        let mulV a b = MS.mulMV a b
        let cptMul a b = MS.cptMulM a b
        let cptMax a b = MS.cptMaxM a b
        let cptMin a b = MS.cptMinM a b
        /// Builds a new matrix where the elements are the result of multiplying every element of the given matrix with the given value
        let scale a b = MS.scaleM a b
        let dot a b = MS.dotM a b
        let neg a = MS.negM a 
        let trace a = MS.traceM a
        let sum a = MS.sumM a
        let prod a = MS.prodM a
        let norm a = MS.normM a
        let transpose a = MS.transM a
        let inplaceAdd a b = MS.inplaceAddM a b
        let inplaceSub a b = MS.inplaceSubM a b

        let exists  f a = MS.existsM  f a
        let forall  f a = MS.forallM  f a
        let existsi  f a = MS.existsiM  f a
        let foralli  f a = MS.foralliM  f a
        let map  f a = MS.mapM f a
        let copy a = MS.copyM a
        let mapi  f a = MS.mapiM f a
        let getDiagN  a n = MS.getDiagnM a n
        let getDiag  a = MS.getDiagnM a 0
        let toDense a = MS.toDenseM a 

        let initDense i j a = MS.initDenseM i j a 
        let initSparse i j a = MS.initSparseM i j a 

        let fold  f z a = MS.foldM f z a
        let foldi f z a = MS.foldiM f z a
          
        let compare a b = MS.compareM LanguagePrimitives.GenericComparer a b
        let hash a      = MS.hashM LanguagePrimitives.GenericEqualityComparer a
        ///Returns row of index i of matrix a
        let getRow    a i           = MS.getRowM a i
        ///Replaces row of index j of matrix a with values of vector v, if vector length matches rowsize
        let setRow (a:Matrix<_>) j (v:Vector<_>) = MS.setRowM a j v
        ///Returns col of index j of matrix a
        let getCol    a j           = MS.getColM a j  
        ///Replaces column of index i of matrix a with values of vector v, if vector length matches columnsize
        let setCol (a:Matrix<_>) i (v:Vector<_>) = MS.setColM a i v
        let getCols   a i1 i2       = MS.getColsM a (i1,i1+i2-1)
        let getRows   a j1 j2       = MS.getRowsM a (j1,j1+j2-1)
        let getRegion a i1 j1 i2 j2 = MS.getRegionM a (i1,i1+i2-1) (j1,j1+j2-1)
            
        let ofRowVector x = MS.rowvecM x
        let ofVector    x = MS.vectorM x
        let toVector    x = MS.toVectorM x
        let toRowVector x = MS.toRowVectorM x
        let toScalar    x = MS.toScalarM x

        let inplace_assign f a  = MS.inplaceAssignM  f a
        let inplace_cptMul a b = MS.inplaceCptMulM a b
        let inplace_scale a b = MS.inplaceScaleM a b
        let inplace_mapi  f a = MS.inplace_mapiM f a
        let of_rowvec x           = ofRowVector x
        let of_vector x           = ofVector x
        let to_vector x           = toVector x
        let to_rowvec x           = toRowVector x
        let to_scalar x           = toScalar x
        let inplace_add a b       = inplaceAdd a b
        let inplace_sub a b       = inplaceSub a b
        let of_scalar   x         = ofScalar x
        let of_list    xss        = ofList xss
        let of_seq     xss        = ofSeq xss
        let inline of_array2D arr = ofArray2D arr
        let inline to_array2D m   = toArray2D m
        let init_diagonal v       = initDiagonal v
        let to_dense a            = toDense a
        let init_dense i j a      = initDense i j a
        let init_sparse i j a     = initSparse i j a
        let nonzero_entries a     = MS.nonZeroEntriesM a 
        
        // TM
        /// Applies function f along row axis 
        let enumerateRowWise f (m:matrix) =
            seq [ 
                for coli=0 to m.NumCols-1 do 
                yield f (seq [for rowi=0 to m.NumRows-1 do yield m.[rowi,coli]])
                ]
        // TM
        /// Applies function f along col�mn axis 
        let enumerateColumnWise f (m:matrix) =
            seq [ 
                for rowi=0 to m.NumRows-1 do 
                yield f (seq [for coli=0 to m.NumCols-1 do yield m.[rowi,coli]])
                ]    

    end

    module MG = Generic
    module DS = DoubleImpl
    module GU = GenericImpl
    module MS = SpecializedGenericImpl

    // Element type OpsData
    type elem = float

    // Accessors
    let get (a:matrix) i j   = MG.get a i j
    let set (a:matrix) i j x = MG.set a i j x
        
    // Creation
    let init  m n f = DS.initDenseMatrixDS  m n f |> MS.dense 
    let ofList    xss   = DS.listDenseMatrixDS    xss |> MS.dense
    let ofSeq     xss   = DS.seqDenseMatrixDS    xss |> MS.dense
    let diag  (v:vector)   = MG.diag v 
    let initDiagonal  (v:vector)   = MG.diag v 
    let constDiag  n x : matrix  = MG.constDiag n x 
    let create  m n x  = DS.constDenseMatrixDS  m n x |> MS.dense
    let ofScalar x     = DS.scalarDenseMatrixDS x |> MS.dense

    let ofArray2D arr : matrix = MG.ofArray2D arr
    let toArray2D (m : matrix) = MG.toArray2D m

    let getDiagN  (a:matrix) n = MG.getDiagN a n
    let getDiag  (a:matrix) = MG.getDiag a

    // Operators
    let add (a:matrix) (b:matrix) = MS.addM   a b
    let sub (a:matrix) (b:matrix) = MS.subM   a b
    let mul (a:matrix) (b:matrix) = MS.mulM   a b
    let mulV (a:matrix) (b:vector) = MS.mulMV   a b
    let mulRV (a:rowvec) (b:matrix) = MS.mulRVM   a b
    let cptMul (a:matrix) (b:matrix) = MS.cptMulM   a b
    let cptMax (a:matrix) (b:matrix) = MS.cptMaxM a b
    let cptMin (a:matrix) (b:matrix) = MS.cptMinM a b
    let scale a (b:matrix) = MS.scaleM   a b
    let neg (a:matrix)  = MS.negM a
    let trace (a:matrix)  = MS.traceM a
    let transpose  (a:matrix) = MG.transpose a
    let forall f (a:matrix) = MG.forall f a
    let exists  f (a:matrix) = MG.exists f a
    let foralli f (a:matrix) = MG.foralli f a
    let existsi  f (a:matrix) = MG.existsi f a
    let map  f (a:matrix) = MG.map f a
    let copy  (a:matrix) = MG.copy a
    let mapi  f (a:matrix) : matrix = MG.mapi f a
    let fold  f z (a:matrix) = MG.fold f z a
    let foldi  f z (a:matrix) = MG.foldi f z a

    let toDense (a:matrix) = MG.toDense a 
    let initDense i j a : matrix = MG.initDense i j a 
    let initSparse i j a : matrix = MG.initSparse i j a 
    let nonzero_entries (a:matrix) = MG.nonzero_entries a 

    let zero m n  = DS.zeroDenseMatrixDS m n |> MS.dense
    let identity m  : matrix = MG.identity m      
    let ones m n  = create m n 1.0
    ///Returns row of index i of matrix a    
    let getRow (a:matrix) i      = MG.getRow a i
    ///Replaces row of index j of matrix a with values of vector v, if vector length matches rowsize
    let setRow (a:Matrix<_>) j (v:Vector<_>) = MG.setRow a j v
    ///Returns col of index j of matrix a
    let getCol (a:matrix) j      = MG.getCol a j
    ///Replaces column of index i of matrix a with values of vector v, if vector length matches columnsize
    let setCol (a:Matrix<_>) i (v:Vector<_>) = MG.setCol a i v
    let getCols (a:matrix) i1 i2    = MG.getCols a i1 i2
    let getRows (a:matrix) j1 j2    = MG.getRows a j1 j2
    let getRegion (a:matrix) i1 j1 i2 j2    = MG.getRegion a i1 j1 i2 j2

    let rowRange (a:Matrix<_>) = (0,a.NumRows - 1)
    let colRange (a:Matrix<_>) = (0,a.NumCols - 1)
    let wholeRegion a = (colRange a, rowRange a)
        
    let foldByRow f (z:Vector<'T>) (a:matrix) = 
        colRange a |> GU.foldR (fun z j -> MS.mapiV (fun i z -> f z (get a i j)) z) z
    let foldByCol f (z:RowVector<'T>) (a:matrix) = 
        rowRange a |> GU.foldR (fun z i -> MS.mapiRV (fun j z -> f z (get a i j)) z) z

    let foldRow f (z:'T) (a:matrix) i = 
        colRange a |> GU.foldR (fun (z:'T) j -> f z (get a i j)) z
    let foldCol f (z:'T) (a:matrix) j = 
        rowRange a |> GU.foldR (fun (z:'T) i -> f z (get a i j)) z

    let sum (a:matrix)  = MS.sumM a
    let prod (a:matrix)  = MS.prodM a
    let norm  (a:matrix) = MS.normM  a
    let dot (a:matrix) b = MS.dotM a b

    let cptPow  a y = map (fun x -> x ** y) a
        
    // Functions that only make sense on this type
    let randomize v = map (fun vij -> MRandom.float vij) v      (* res_ij = random [0,vij] values *)

    let ofRowVector x : matrix = MS.rowvecM x
    let ofVector    x : matrix = MS.vectorM x
    let toVector    x : vector = MS.toVectorM x
    let toRowVector x : rowvec = MS.toRowVectorM x
    let toScalar    x : float  = MS.toScalarM x

    let inplaceAdd  (a:matrix) b = MS.inplaceAddM a b
    let inplaceSub  (a:matrix) b = MS.inplaceSubM a b

    // Mutation
    let inplace_assign  f (a:matrix) = MG.inplace_assign f a
    let inplace_mapi  f (a:matrix) = MG.inplace_mapi f a
    let inplace_cptMul (a:matrix) b = MS.inplaceCptMulM a b
    let inplace_scale  a (b:matrix) = MS.inplaceScaleM a b

    let inplace_add  a b = inplaceAdd a b
    let inplace_sub  a b = inplaceSub a b
    let of_rowvec x = ofRowVector x
    let of_vector x = ofVector x
    let to_vector x = toVector x
    let to_rowvec x = toRowVector x
    let to_scalar x = toScalar x
    let inline of_array2D arr  = ofArray2D arr
    let inline to_array2D m = toArray2D m
    let of_list    xss   = ofList xss
    let of_seq     xss   = ofSeq xss
    let init_diagonal v   = initDiagonal   v
    let of_scalar x     = ofScalar x
    let to_dense x = toDense x
    let init_dense i j a = initDense i j a
    let init_sparse i j a = initSparse i j a

    //----------------------------------------------------------------------------
    // Stats

    /// Returns upper triangular Matrix by setting all values beneath the diagonal to Zero.  
    /// Warning: triangular matrices can only be computed for square input matrices.
    let getUpperTriangular (a:Matrix<float>) = 
        let nA = a.NumCols 
        let mA = a.NumRows
        if nA<>mA then invalidArg "a" "expected a square matrix";
        else
        a  
        |> mapi (fun n m x -> if n > m then 0. else x ) 
    
    /// Returns lower triangular Matrix by setting all values beneath the diagonal to Zero.
    /// Warning: triangular matrices can only be computed for square input matrices.
    let getLowerTriangular (a:Matrix<float>)  = 
        let nA = a.NumCols 
        let mA = a.NumRows
        if nA<>mA then invalidArg "a" "expected a square matrix";
        else
        a  
        |> mapi (fun n m x -> if n < m then 0. else x ) 

    /// Returns diagonal matrix by setting all values beneath and above the diagonal to Zero.
    /// Warning: diagonal matrices can only be computed for square input matrices.
    let toDiagonal (a:Matrix<float>) = 
        getDiag a
        |> diag

    /// Computes the row wise mean of a Matrix 
    let meanRowWise (a:matrix) = 
        a
        |> foldByRow (fun acc r -> acc + r ) (Vector.zero a.NumRows)
        |> Vector.map (fun sum -> sum / (a.NumRows |> float))
    
    /// Computes the Column wise mean of a Matrix 
    let meanColumnWise (a:matrix) = 
        a.Transpose 
        |> foldByRow (fun acc r -> acc + r ) (Vector.zero a.NumCols)
        |> Vector.map (fun sum -> sum / (a.NumCols |> float))
    

    /// computes the column specific covariance matrix of a data matrix as described at:
    // http://stattrek.com/matrix-algebra/covariance-matrix.aspx
    let columnCovarianceMatrixOf df (dataMatrix:Matrix<float>) =
        /// Step 1:
        /// contains the deviation scores for the data matrix 
        let devMatrix =
            let ident = ones dataMatrix.NumRows dataMatrix.NumRows
            dataMatrix - (ident * dataMatrix |> map (fun elem -> elem / float dataMatrix.NumRows))
        /// Step 2:
        /// Compute devMatrix' * devMatrix, the k x k deviation sums of squares and cross products matrix for x.
        let devMTdevM =
            devMatrix.Transpose * devMatrix
        /// Step 3: 
        /// Then, divide each term in the deviation sums of squares and cross product matrix by n to create the variance-covariance matrix. That is:
        devMTdevM |> map (fun elem -> elem / (float df))

    /// computes the column specific population covariance matrix of a data matrix 
    let columnPopulationCovarianceMatrixOf (dataMatrix:Matrix<float>) =
        columnCovarianceMatrixOf dataMatrix.NumRows dataMatrix

    /// computes the column specific sample covariance matrix of a data matrix
    let columnSampleCovarianceMatrixOf (dataMatrix:Matrix<float>) =
        columnCovarianceMatrixOf (dataMatrix.NumRows-1) dataMatrix

    /// computes the row specific population covariance matrix of a data matrix 
    let rowPopulationCovarianceMatrixOf (dataMatrix:Matrix<float>) =
        columnCovarianceMatrixOf dataMatrix.Transpose.NumRows dataMatrix.Transpose

    /// computes the row specific sample covariance matrix of a data matrix 
    let rowSampleCovarianceMatrixOf (dataMatrix:Matrix<float>) =
        columnCovarianceMatrixOf (dataMatrix.Transpose.NumRows-1) dataMatrix.Transpose

    //----------------------------------------------------------------------------

end

[<AutoOpen>]
module MatrixExtension =

    type Matrix<'T> with 
        member x.ToArray2()        = Matrix.Generic.toArray2D x
        member x.ToArray2D()        = Matrix.Generic.toArray2D x

#if FX_NO_DEBUG_DISPLAYS
#else
        [<DebuggerBrowsable(DebuggerBrowsableState.Collapsed)>]
#endif

        member x.NonZeroEntries    = Matrix.Generic.nonzero_entries x
        member x.ToScalar()        = Matrix.Generic.toScalar x
        member x.ToRowVector()     = Matrix.Generic.toRowVector x               
        member x.ToVector()        = Matrix.Generic.toVector x

#if FX_NO_DEBUG_DISPLAYS
#else
        [<DebuggerBrowsable(DebuggerBrowsableState.Collapsed)>]
#endif
        member x.Norm              = Matrix.Generic.norm x

        member x.Column(n)         = Matrix.Generic.getCol x n
        member x.Row(n)            = Matrix.Generic.getRow x n
        member x.Columns (i,ni)    = Matrix.Generic.getCols x i ni
        member x.Rows (j,nj)       = Matrix.Generic.getRows x j nj
        member x.Region(i,j,ni,nj) = Matrix.Generic.getRegion x i j ni nj
        member x.GetDiagonal(i)    = Matrix.Generic.getDiagN x i

#if FX_NO_DEBUG_DISPLAYS
#else
        [<DebuggerBrowsable(DebuggerBrowsableState.Collapsed)>]
#endif
        member x.Diagonal          = Matrix.Generic.getDiag x

        member x.Copy () = Matrix.Generic.copy x


//    [<AutoOpen>]
//    module MatrixTopLevelOperators = 
//
//        let matrix ll = Matrix.ofSeq ll
//        let vector l  = Vector.ofSeq  l
//        let rowvec l  = RowVector.ofSeq l

