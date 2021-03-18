namespace MonoEng
open Microsoft.Xna.Framework.Graphics
open SharpDX.Direct3D11
module GPUUtil =
  let r32Typeless = SharpDX.DXGI.Format.R32_Typeless
  //let skinBuf device = new Buffer(device,)
  let bufDec sizeByte usage bind cpu option stride =
    let mutable desc = new BufferDescription()
    desc.SizeInBytes <- sizeByte
    desc.Usage <- usage
    desc.BindFlags <- bind
    desc.CpuAccessFlags <- cpu
    desc.OptionFlags <- option
    desc.StructureByteStride <- stride
    desc
  let srvDec format dim ex =
    let mutable desc = new ShaderResourceViewDescription()
    desc.Format <- format
    desc.Dimension<- dim
    desc.BufferEx <- ex
    desc

  let exBufDec fstElem elemCnt =
    let mutable desc = new ShaderResourceViewDescription.ExtendedBufferResource()
    desc.FirstElement <- fstElem
    desc.ElementCount <- elemCnt
    desc

  let uavDesc format dim buf =
    let mutable desc = new UnorderedAccessViewDescription()
    desc.Format <- format
    desc.Dimension<- dim
    desc.Buffer <- buf
    desc

  let uavBufDesc fstElem elemCnt flag=
    let mutable desc = new UnorderedAccessViewDescription.BufferResource()
    desc.FirstElement <- fstElem
    desc.ElementCount <- elemCnt
    desc.Flags <- flag
    desc
(*
機械生成
open System
open System.Reflection
let low (s:string)= s.[0].ToString().ToLower() + s.[1..]
type FieInfo =
  {
    TName : string
    Args :string list
    Body :string list
  }
  member t.ToString() =
    "  let " + low t.TName + (t.Args |> List.fold (fun acc i->acc + " " + i) "") + " =\n" + "    let mutable desc = new " + t.TName + "()" + (t.Body|> List.fold (fun acc i->acc + "\n" + i) "") + "\n    desc"

let fieIn name (infos : FieldInfo[]) =
  let args = [for i in infos -> low i.Name]
  let bodys = [for i in infos -> "    desc." + i.Name + " <- " + (low i.Name)]
  {TName = name ;Args = args;Body = bodys}

    let assemDx = Assembly.Load("SharpDX.Direct3D11")
    let types = assemDx.GetTypes()
    let typ = types |> Array.filter (fun (t:Type)->t.IsValueType && not t.IsEnum && not t.IsPrimitive && not (t.Name.StartsWith("__")) )// && not (t.Namespace.StartsWith("System")) )
    let fieldName = typ |> Array.map (fun t-> fieIn t.Name  (t.GetFields()) ) //|> Array.concat
    for i in fieldName do
      System.Diagnostics.Debug.WriteLine <|i.ToString()
*)
  let blendStateDescription alphaToCoverageEnable independentBlendEnable =
    let mutable desc = new BlendStateDescription()
    desc.AlphaToCoverageEnable <- alphaToCoverageEnable
    desc.IndependentBlendEnable <- independentBlendEnable
    desc
  let blendStateDescription1 alphaToCoverageEnable independentBlendEnable =
    let mutable desc = new BlendStateDescription1()
    desc.AlphaToCoverageEnable <- alphaToCoverageEnable
    desc.IndependentBlendEnable <- independentBlendEnable
    desc
  let bufferDescription sizeInBytes usage bindFlags cpuAccessFlags optionFlags structureByteStride =
    let mutable desc = new BufferDescription()
    desc.SizeInBytes <- sizeInBytes
    desc.Usage <- usage
    desc.BindFlags <- bindFlags
    desc.CpuAccessFlags <- cpuAccessFlags
    desc.OptionFlags <- optionFlags
    desc.StructureByteStride <- structureByteStride
    desc
  let depthStencilStateDescription isDepthEnabled depthWriteMask depthComparison isStencilEnabled stencilReadMask stencilWriteMask frontFace backFace =
    let mutable desc = new DepthStencilStateDescription()
    desc.IsDepthEnabled <- isDepthEnabled
    desc.DepthWriteMask <- depthWriteMask
    desc.DepthComparison <- depthComparison
    desc.IsStencilEnabled <- isStencilEnabled
    desc.StencilReadMask <- stencilReadMask
    desc.StencilWriteMask <- stencilWriteMask
    desc.FrontFace <- frontFace
    desc.BackFace <- backFace
    desc
  let aesCtrIv iv count =
    let mutable desc = new AesCtrIv()
    desc.Iv <- iv
    desc.Count <- count
    desc
  let authenticatedConfigureAccessibleEncryptionInput parameters encryptionGuid =
    let mutable desc = new AuthenticatedConfigureAccessibleEncryptionInput()
    desc.Parameters <- parameters
    desc.EncryptionGuid <- encryptionGuid
    desc
  let authenticatedConfigureCryptoSessionInput parameters decoderHandle cryptoSessionHandle deviceHandle =
    let mutable desc = new AuthenticatedConfigureCryptoSessionInput()
    desc.Parameters <- parameters
    desc.DecoderHandle <- decoderHandle
    desc.CryptoSessionHandle <- cryptoSessionHandle
    desc.DeviceHandle <- deviceHandle
    desc
  let authenticatedConfigureInitializeInput parameters startSequenceQuery startSequenceConfigure =
    let mutable desc = new AuthenticatedConfigureInitializeInput()
    desc.Parameters <- parameters
    desc.StartSequenceQuery <- startSequenceQuery
    desc.StartSequenceConfigure <- startSequenceConfigure
    desc
  let authenticatedConfigureInput omac configureType hChannel sequenceNumber =
    let mutable desc = new AuthenticatedConfigureInput()
    desc.Omac <- omac
    desc.ConfigureType <- configureType
    desc.HChannel <- hChannel
    desc.SequenceNumber <- sequenceNumber
    desc
  let authenticatedConfigureOutput omac configureType hChannel sequenceNumber returnCode =
    let mutable desc = new AuthenticatedConfigureOutput()
    desc.Omac <- omac
    desc.ConfigureType <- configureType
    desc.HChannel <- hChannel
    desc.SequenceNumber <- sequenceNumber
    desc.ReturnCode <- returnCode
    desc
  let authenticatedConfigureProtectionInput parameters protections =
    let mutable desc = new AuthenticatedConfigureProtectionInput()
    desc.Parameters <- parameters
    desc.Protections <- protections
    desc
  let authenticatedConfigureSharedResourceInput parameters processType processHandle allowAccess =
    let mutable desc = new AuthenticatedConfigureSharedResourceInput()
    desc.Parameters <- parameters
    desc.ProcessType <- processType
    desc.ProcessHandle <- processHandle
    desc.AllowAccess <- allowAccess
    desc
  let authenticatedProtectionFlags flags value =
    let mutable desc = new AuthenticatedProtectionFlags()
    desc.Flags <- flags
    desc.Value <- value
    desc
  let authenticatedProtectionFlagsMidlMidlItfD3d11000000340001Inner =
    let mutable desc = new AuthenticatedProtectionFlagsMidlMidlItfD3d11000000340001Inner()
    desc
  let authenticatedQueryAccessibilityEncryptionGuidCountOutput output encryptionGuidCount =
    let mutable desc = new AuthenticatedQueryAccessibilityEncryptionGuidCountOutput()
    desc.Output <- output
    desc.EncryptionGuidCount <- encryptionGuidCount
    desc
  let authenticatedQueryAccessibilityEncryptionGuidInput input encryptionGuidIndex =
    let mutable desc = new AuthenticatedQueryAccessibilityEncryptionGuidInput()
    desc.Input <- input
    desc.EncryptionGuidIndex <- encryptionGuidIndex
    desc
  let authenticatedQueryAccessibilityEncryptionGuidOutput output encryptionGuidIndex encryptionGuid =
    let mutable desc = new AuthenticatedQueryAccessibilityEncryptionGuidOutput()
    desc.Output <- output
    desc.EncryptionGuidIndex <- encryptionGuidIndex
    desc.EncryptionGuid <- encryptionGuid
    desc
  let authenticatedQueryAcessibilityOutput output busType accessibleInContiguousBlocks accessibleInNonContiguousBlocks =
    let mutable desc = new AuthenticatedQueryAcessibilityOutput()
    desc.Output <- output
    desc.BusType <- busType
    desc.AccessibleInContiguousBlocks <- accessibleInContiguousBlocks
    desc.AccessibleInNonContiguousBlocks <- accessibleInNonContiguousBlocks
    desc
  let authenticatedQueryChannelTypeOutput output channelType =
    let mutable desc = new AuthenticatedQueryChannelTypeOutput()
    desc.Output <- output
    desc.ChannelType <- channelType
    desc
  let authenticatedQueryCryptoSessionInput input decoderHandle =
    let mutable desc = new AuthenticatedQueryCryptoSessionInput()
    desc.Input <- input
    desc.DecoderHandle <- decoderHandle
    desc
  let authenticatedQueryCryptoSessionOutput output decoderHandle cryptoSessionHandle deviceHandle =
    let mutable desc = new AuthenticatedQueryCryptoSessionOutput()
    desc.Output <- output
    desc.DecoderHandle <- decoderHandle
    desc.CryptoSessionHandle <- cryptoSessionHandle
    desc.DeviceHandle <- deviceHandle
    desc
  let authenticatedQueryCurrentAccessibilityEncryptionOutput output encryptionGuid =
    let mutable desc = new AuthenticatedQueryCurrentAccessibilityEncryptionOutput()
    desc.Output <- output
    desc.EncryptionGuid <- encryptionGuid
    desc
  let authenticatedQueryDeviceHandleOutput output deviceHandle =
    let mutable desc = new AuthenticatedQueryDeviceHandleOutput()
    desc.Output <- output
    desc.DeviceHandle <- deviceHandle
    desc
  let authenticatedQueryInput queryType hChannel sequenceNumber =
    let mutable desc = new AuthenticatedQueryInput()
    desc.QueryType <- queryType
    desc.HChannel <- hChannel
    desc.SequenceNumber <- sequenceNumber
    desc
  let authenticatedQueryOutput omac queryType hChannel sequenceNumber returnCode =
    let mutable desc = new AuthenticatedQueryOutput()
    desc.Omac <- omac
    desc.QueryType <- queryType
    desc.HChannel <- hChannel
    desc.SequenceNumber <- sequenceNumber
    desc.ReturnCode <- returnCode
    desc
  let authenticatedQueryOutputIdCountInput input deviceHandle cryptoSessionHandle =
    let mutable desc = new AuthenticatedQueryOutputIdCountInput()
    desc.Input <- input
    desc.DeviceHandle <- deviceHandle
    desc.CryptoSessionHandle <- cryptoSessionHandle
    desc
  let authenticatedQueryOutputIdCountOutput output deviceHandle cryptoSessionHandle outputIDCount =
    let mutable desc = new AuthenticatedQueryOutputIdCountOutput()
    desc.Output <- output
    desc.DeviceHandle <- deviceHandle
    desc.CryptoSessionHandle <- cryptoSessionHandle
    desc.OutputIDCount <- outputIDCount
    desc
  let authenticatedQueryOutputIdInput input deviceHandle cryptoSessionHandle outputIDIndex =
    let mutable desc = new AuthenticatedQueryOutputIdInput()
    desc.Input <- input
    desc.DeviceHandle <- deviceHandle
    desc.CryptoSessionHandle <- cryptoSessionHandle
    desc.OutputIDIndex <- outputIDIndex
    desc
  let authenticatedQueryOutputIdOutput output deviceHandle cryptoSessionHandle outputIDIndex outputID =
    let mutable desc = new AuthenticatedQueryOutputIdOutput()
    desc.Output <- output
    desc.DeviceHandle <- deviceHandle
    desc.CryptoSessionHandle <- cryptoSessionHandle
    desc.OutputIDIndex <- outputIDIndex
    desc.OutputID <- outputID
    desc
  let authenticatedQueryProtectionOutput output protectionFlags =
    let mutable desc = new AuthenticatedQueryProtectionOutput()
    desc.Output <- output
    desc.ProtectionFlags <- protectionFlags
    desc
  let authenticatedQueryRestrictedSharedResourceProcessCountOutput output restrictedSharedResourceProcessCount =
    let mutable desc = new AuthenticatedQueryRestrictedSharedResourceProcessCountOutput()
    desc.Output <- output
    desc.RestrictedSharedResourceProcessCount <- restrictedSharedResourceProcessCount
    desc
  let authenticatedQueryRestrictedSharedResourceProcessInput input processIndex =
    let mutable desc = new AuthenticatedQueryRestrictedSharedResourceProcessInput()
    desc.Input <- input
    desc.ProcessIndex <- processIndex
    desc
  let authenticatedQueryRestrictedSharedResourceProcessOutput output processIndex processIdentifier processHandle =
    let mutable desc = new AuthenticatedQueryRestrictedSharedResourceProcessOutput()
    desc.Output <- output
    desc.ProcessIndex <- processIndex
    desc.ProcessIdentifier <- processIdentifier
    desc.ProcessHandle <- processHandle
    desc
  let authenticatedQueryUnrestrictedProtectedSharedResourceCountOutput output unrestrictedProtectedSharedResourceCount =
    let mutable desc = new AuthenticatedQueryUnrestrictedProtectedSharedResourceCountOutput()
    desc.Output <- output
    desc.UnrestrictedProtectedSharedResourceCount <- unrestrictedProtectedSharedResourceCount
    desc
  let cd3d11VideoDefault =
    let mutable desc = new Cd3d11VideoDefault()
    desc
  let classInstanceDescription instanceId instanceIndex typeId constantBuffer baseConstantBufferOffset baseTexture baseSampler isCreated =
    let mutable desc = new ClassInstanceDescription()
    desc.InstanceId <- instanceId
    desc.InstanceIndex <- instanceIndex
    desc.TypeId <- typeId
    desc.ConstantBuffer <- constantBuffer
    desc.BaseConstantBufferOffset <- baseConstantBufferOffset
    desc.BaseTexture <- baseTexture
    desc.BaseSampler <- baseSampler
    desc.IsCreated <- isCreated
    desc
  let counterCapabilities lastDeviceDependentCounter simultaneousCounterCount detectableParallelUnitCount =
    let mutable desc = new CounterCapabilities()
    desc.LastDeviceDependentCounter <- lastDeviceDependentCounter
    desc.SimultaneousCounterCount <- simultaneousCounterCount
    desc.DetectableParallelUnitCount <- detectableParallelUnitCount
    desc
  let counterDescription counter miscFlags =
    let mutable desc = new CounterDescription()
    desc.Counter <- counter
    desc.MiscFlags <- miscFlags
    desc
  let d3D11ResourceFlags bindFlags miscFlags cPUAccessFlags structureByteStride =
    let mutable desc = new D3D11ResourceFlags()
    desc.BindFlags <- bindFlags
    desc.MiscFlags <- miscFlags
    desc.CPUAccessFlags <- cPUAccessFlags
    desc.StructureByteStride <- structureByteStride
    desc
  let depthStencilOperationDescription failOperation depthFailOperation passOperation comparison =
    let mutable desc = new DepthStencilOperationDescription()
    desc.FailOperation <- failOperation
    desc.DepthFailOperation <- depthFailOperation
    desc.PassOperation <- passOperation
    desc.Comparison <- comparison
    desc
  let depthStencilViewDescription format dimension flags texture1D texture1DArray texture2D texture2DArray texture2DMS texture2DMSArray =
    let mutable desc = new DepthStencilViewDescription()
    desc.Format <- format
    desc.Dimension <- dimension
    desc.Flags <- flags
    desc.Texture1D <- texture1D
    desc.Texture1DArray <- texture1DArray
    desc.Texture2D <- texture2D
    desc.Texture2DArray <- texture2DArray
    desc.Texture2DMS <- texture2DMS
    desc.Texture2DMSArray <- texture2DMSArray
    desc
  let drawIndexedInstancedIndirectArguments indexCountPerInstance instanceCount startIndexLocation baseVertexLocation startInstanceLocation =
    let mutable desc = new DrawIndexedInstancedIndirectArguments()
    desc.IndexCountPerInstance <- indexCountPerInstance
    desc.InstanceCount <- instanceCount
    desc.StartIndexLocation <- startIndexLocation
    desc.BaseVertexLocation <- baseVertexLocation
    desc.StartInstanceLocation <- startInstanceLocation
    desc
  let drawInstancedIndirectArguments vertexCountPerInstance instanceCount startVertexLocation startInstanceLocation =
    let mutable desc = new DrawInstancedIndirectArguments()
    desc.VertexCountPerInstance <- vertexCountPerInstance
    desc.InstanceCount <- instanceCount
    desc.StartVertexLocation <- startVertexLocation
    desc.StartInstanceLocation <- startInstanceLocation
    desc
  let encryptedBlockInformation numEncryptedBytesAtBeginning numBytesInSkipPattern numBytesInEncryptPattern =
    let mutable desc = new EncryptedBlockInformation()
    desc.NumEncryptedBytesAtBeginning <- numEncryptedBytesAtBeginning
    desc.NumBytesInSkipPattern <- numBytesInSkipPattern
    desc.NumBytesInEncryptPattern <- numBytesInEncryptPattern
    desc
  let featureDataD3D11Options outputMergerLogicOp uAVOnlyRenderingForcedSampleCount discardAPIsSeenByDriver flagsForUpdateAndCopySeenByDriver clearView copyWithOverlap constantBufferPartialUpdate constantBufferOffsetting mapNoOverwriteOnDynamicConstantBuffer mapNoOverwriteOnDynamicBufferSRV multisampleRTVWithForcedSampleCountOne sAD4ShaderInstructions extendedDoublesShaderInstructions extendedResourceSharing =
    let mutable desc = new FeatureDataD3D11Options()
    desc.OutputMergerLogicOp <- outputMergerLogicOp
    desc.UAVOnlyRenderingForcedSampleCount <- uAVOnlyRenderingForcedSampleCount
    desc.DiscardAPIsSeenByDriver <- discardAPIsSeenByDriver
    desc.FlagsForUpdateAndCopySeenByDriver <- flagsForUpdateAndCopySeenByDriver
    desc.ClearView <- clearView
    desc.CopyWithOverlap <- copyWithOverlap
    desc.ConstantBufferPartialUpdate <- constantBufferPartialUpdate
    desc.ConstantBufferOffsetting <- constantBufferOffsetting
    desc.MapNoOverwriteOnDynamicConstantBuffer <- mapNoOverwriteOnDynamicConstantBuffer
    desc.MapNoOverwriteOnDynamicBufferSRV <- mapNoOverwriteOnDynamicBufferSRV
    desc.MultisampleRTVWithForcedSampleCountOne <- multisampleRTVWithForcedSampleCountOne
    desc.SAD4ShaderInstructions <- sAD4ShaderInstructions
    desc.ExtendedDoublesShaderInstructions <- extendedDoublesShaderInstructions
    desc.ExtendedResourceSharing <- extendedResourceSharing
    desc
  let featureDataD3D11Options1 tiledResourcesTier minMaxFiltering clearViewAlsoSupportsDepthOnlyFormats mapOnDefaultBuffers =
    let mutable desc = new FeatureDataD3D11Options1()
    desc.TiledResourcesTier <- tiledResourcesTier
    desc.MinMaxFiltering <- minMaxFiltering
    desc.ClearViewAlsoSupportsDepthOnlyFormats <- clearViewAlsoSupportsDepthOnlyFormats
    desc.MapOnDefaultBuffers <- mapOnDefaultBuffers
    desc
  let featureDataD3D11Options2 pSSpecifiedStencilRefSupported typedUAVLoadAdditionalFormats rOVsSupported conservativeRasterizationTier tiledResourcesTier mapOnDefaultTextures standardSwizzle unifiedMemoryArchitecture =
    let mutable desc = new FeatureDataD3D11Options2()
    desc.PSSpecifiedStencilRefSupported <- pSSpecifiedStencilRefSupported
    desc.TypedUAVLoadAdditionalFormats <- typedUAVLoadAdditionalFormats
    desc.ROVsSupported <- rOVsSupported
    desc.ConservativeRasterizationTier <- conservativeRasterizationTier
    desc.TiledResourcesTier <- tiledResourcesTier
    desc.MapOnDefaultTextures <- mapOnDefaultTextures
    desc.StandardSwizzle <- standardSwizzle
    desc.UnifiedMemoryArchitecture <- unifiedMemoryArchitecture
    desc
  let featureDataD3D11Options3 vPAndRTArrayIndexFromAnyShaderFeedingRasterizer =
    let mutable desc = new FeatureDataD3D11Options3()
    desc.VPAndRTArrayIndexFromAnyShaderFeedingRasterizer <- vPAndRTArrayIndexFromAnyShaderFeedingRasterizer
    desc
  let featureDataD3D11Options4 extendedNV12SharedTextureSupported =
    let mutable desc = new FeatureDataD3D11Options4()
    desc.ExtendedNV12SharedTextureSupported <- extendedNV12SharedTextureSupported
    desc
  let featureDataShaderMinimumPrecisionSupport pixelShaderMinPrecision allOtherShaderStagesMinPrecision =
    let mutable desc = new FeatureDataShaderMinimumPrecisionSupport()
    desc.PixelShaderMinPrecision <- pixelShaderMinPrecision
    desc.AllOtherShaderStagesMinPrecision <- allOtherShaderStagesMinPrecision
    desc
  let inputElement semanticName semanticIndex format slot alignedByteOffset classification instanceDataStepRate =
    let mutable desc = new InputElement()
    desc.SemanticName <- semanticName
    desc.SemanticIndex <- semanticIndex
    desc.Format <- format
    desc.Slot <- slot
    desc.AlignedByteOffset <- alignedByteOffset
    desc.Classification <- classification
    desc.InstanceDataStepRate <- instanceDataStepRate
    desc
  let keyExchangeHwProtectionData hWProtectionFunctionID pInputData pOutputData status =
    let mutable desc = new KeyExchangeHwProtectionData()
    desc.HWProtectionFunctionID <- hWProtectionFunctionID
    desc.PInputData <- pInputData
    desc.POutputData <- pOutputData
    desc.Status <- status
    desc
  let keyExchangeHwProtectionInputData privateDataSize hWProtectionDataSize =
    let mutable desc = new KeyExchangeHwProtectionInputData()
    desc.PrivateDataSize <- privateDataSize
    desc.HWProtectionDataSize <- hWProtectionDataSize
    desc
  let keyExchangeHwProtectionOutputData privateDataSize maxHWProtectionDataSize hWProtectionDataSize transportTime executionTime =
    let mutable desc = new KeyExchangeHwProtectionOutputData()
    desc.PrivateDataSize <- privateDataSize
    desc.MaxHWProtectionDataSize <- maxHWProtectionDataSize
    desc.HWProtectionDataSize <- hWProtectionDataSize
    desc.TransportTime <- transportTime
    desc.ExecutionTime <- executionTime
    desc
  let message category severity id description =
    let mutable desc = new Message()
    desc.Category <- category
    desc.Severity <- severity
    desc.Id <- id
    desc.Description <- description
    desc
  let messageAuthenticationCode =
    let mutable desc = new MessageAuthenticationCode()
    desc
  let packedMipDescription standardMipCount packedMipCount tilesForPackedMipCount startTileIndexInOverallResource =
    let mutable desc = new PackedMipDescription()
    desc.StandardMipCount <- standardMipCount
    desc.PackedMipCount <- packedMipCount
    desc.TilesForPackedMipCount <- tilesForPackedMipCount
    desc.StartTileIndexInOverallResource <- startTileIndexInOverallResource
    desc
  let queryDataPipelineStatistics iAVerticeCount iAPrimitiveCount vSInvocationCount gSInvocationCount gSPrimitiveCount cInvocationCount cPrimitiveCount pSInvocationCount hSInvocationCount dSInvocationCount cSInvocationCount =
    let mutable desc = new QueryDataPipelineStatistics()
    desc.IAVerticeCount <- iAVerticeCount
    desc.IAPrimitiveCount <- iAPrimitiveCount
    desc.VSInvocationCount <- vSInvocationCount
    desc.GSInvocationCount <- gSInvocationCount
    desc.GSPrimitiveCount <- gSPrimitiveCount
    desc.CInvocationCount <- cInvocationCount
    desc.CPrimitiveCount <- cPrimitiveCount
    desc.PSInvocationCount <- pSInvocationCount
    desc.HSInvocationCount <- hSInvocationCount
    desc.DSInvocationCount <- dSInvocationCount
    desc.CSInvocationCount <- cSInvocationCount
    desc
  let queryDataTimestampDisjoint frequency disjoint =
    let mutable desc = new QueryDataTimestampDisjoint()
    desc.Frequency <- frequency
    desc.Disjoint <- disjoint
    desc
  let queryDescription typen flags =
    let mutable desc = new QueryDescription()
    desc.Type <- typen
    desc.Flags <- flags
    desc
  let queryDescription1 query miscFlags contextType =
    let mutable desc = new QueryDescription1()
    desc.Query <- query
    desc.MiscFlags <- miscFlags
    desc.ContextType <- contextType
    desc
  let rasterizerStateDescription fillMode cullMode isFrontCounterClockwise depthBias depthBiasClamp slopeScaledDepthBias isDepthClipEnabled isScissorEnabled isMultisampleEnabled isAntialiasedLineEnabled =
    let mutable desc = new RasterizerStateDescription()
    desc.FillMode <- fillMode
    desc.CullMode <- cullMode
    desc.IsFrontCounterClockwise <- isFrontCounterClockwise
    desc.DepthBias <- depthBias
    desc.DepthBiasClamp <- depthBiasClamp
    desc.SlopeScaledDepthBias <- slopeScaledDepthBias
    desc.IsDepthClipEnabled <- isDepthClipEnabled
    desc.IsScissorEnabled <- isScissorEnabled
    desc.IsMultisampleEnabled <- isMultisampleEnabled
    desc.IsAntialiasedLineEnabled <- isAntialiasedLineEnabled
    desc
  let rasterizerStateDescription1 fillMode cullMode isFrontCounterClockwise depthBias depthBiasClamp slopeScaledDepthBias isDepthClipEnabled isScissorEnabled isMultisampleEnabled isAntialiasedLineEnabled forcedSampleCount =
    let mutable desc = new RasterizerStateDescription1()
    desc.FillMode <- fillMode
    desc.CullMode <- cullMode
    desc.IsFrontCounterClockwise <- isFrontCounterClockwise
    desc.DepthBias <- depthBias
    desc.DepthBiasClamp <- depthBiasClamp
    desc.SlopeScaledDepthBias <- slopeScaledDepthBias
    desc.IsDepthClipEnabled <- isDepthClipEnabled
    desc.IsScissorEnabled <- isScissorEnabled
    desc.IsMultisampleEnabled <- isMultisampleEnabled
    desc.IsAntialiasedLineEnabled <- isAntialiasedLineEnabled
    desc.ForcedSampleCount <- forcedSampleCount
    desc
  let rasterizerStateDescription2 fillMode cullMode isFrontCounterClockwise depthBias depthBiasClamp slopeScaledDepthBias isDepthClipEnabled isScissorEnabled isMultisampleEnabled isAntialiasedLineEnabled forcedSampleCount conservativeRasterizationMode =
    let mutable desc = new RasterizerStateDescription2()
    desc.FillMode <- fillMode
    desc.CullMode <- cullMode
    desc.IsFrontCounterClockwise <- isFrontCounterClockwise
    desc.DepthBias <- depthBias
    desc.DepthBiasClamp <- depthBiasClamp
    desc.SlopeScaledDepthBias <- slopeScaledDepthBias
    desc.IsDepthClipEnabled <- isDepthClipEnabled
    desc.IsScissorEnabled <- isScissorEnabled
    desc.IsMultisampleEnabled <- isMultisampleEnabled
    desc.IsAntialiasedLineEnabled <- isAntialiasedLineEnabled
    desc.ForcedSampleCount <- forcedSampleCount
    desc.ConservativeRasterizationMode <- conservativeRasterizationMode
    desc
  let renderTargetBlendDescription isBlendEnabled sourceBlend destinationBlend blendOperation sourceAlphaBlend destinationAlphaBlend alphaBlendOperation renderTargetWriteMask =
    let mutable desc = new RenderTargetBlendDescription()
    desc.IsBlendEnabled <- isBlendEnabled
    desc.SourceBlend <- sourceBlend
    desc.DestinationBlend <- destinationBlend
    desc.BlendOperation <- blendOperation
    desc.SourceAlphaBlend <- sourceAlphaBlend
    desc.DestinationAlphaBlend <- destinationAlphaBlend
    desc.AlphaBlendOperation <- alphaBlendOperation
    desc.RenderTargetWriteMask <- renderTargetWriteMask
    desc
  let renderTargetBlendDescription1 isBlendEnabled isLogicOperationEnabled sourceBlend destinationBlend blendOperation sourceAlphaBlend destinationAlphaBlend alphaBlendOperation logicOperation renderTargetWriteMask =
    let mutable desc = new RenderTargetBlendDescription1()
    desc.IsBlendEnabled <- isBlendEnabled
    desc.IsLogicOperationEnabled <- isLogicOperationEnabled
    desc.SourceBlend <- sourceBlend
    desc.DestinationBlend <- destinationBlend
    desc.BlendOperation <- blendOperation
    desc.SourceAlphaBlend <- sourceAlphaBlend
    desc.DestinationAlphaBlend <- destinationAlphaBlend
    desc.AlphaBlendOperation <- alphaBlendOperation
    desc.LogicOperation <- logicOperation
    desc.RenderTargetWriteMask <- renderTargetWriteMask
    desc
  let renderTargetViewDescription format dimension buffer texture1D texture1DArray texture2D texture2DArray texture2DMS texture2DMSArray texture3D =
    let mutable desc = new RenderTargetViewDescription()
    desc.Format <- format
    desc.Dimension <- dimension
    desc.Buffer <- buffer
    desc.Texture1D <- texture1D
    desc.Texture1DArray <- texture1DArray
    desc.Texture2D <- texture2D
    desc.Texture2DArray <- texture2DArray
    desc.Texture2DMS <- texture2DMS
    desc.Texture2DMSArray <- texture2DMSArray
    desc.Texture3D <- texture3D
    desc
  let renderTargetViewDescription1 format dimension buffer texture1D texture1DArray texture2D texture2DArray texture2DMS texture2DMSArray texture3D =
    let mutable desc = new RenderTargetViewDescription1()
    desc.Format <- format
    desc.Dimension <- dimension
    desc.Buffer <- buffer
    desc.Texture1D <- texture1D
    desc.Texture1DArray <- texture1DArray
    desc.Texture2D <- texture2D
    desc.Texture2DArray <- texture2DArray
    desc.Texture2DMS <- texture2DMS
    desc.Texture2DMSArray <- texture2DMSArray
    desc.Texture3D <- texture3D
    desc
  let resourceRegion left top front right bottom back =
    let mutable desc = new ResourceRegion()
    desc.Left <- left
    desc.Top <- top
    desc.Front <- front
    desc.Right <- right
    desc.Bottom <- bottom
    desc.Back <- back
    desc
  let samplerStateDescription filter addressU addressV addressW mipLodBias maximumAnisotropy comparisonFunction borderColor minimumLod maximumLod =
    let mutable desc = new SamplerStateDescription()
    desc.Filter <- filter
    desc.AddressU <- addressU
    desc.AddressV <- addressV
    desc.AddressW <- addressW
    desc.MipLodBias <- mipLodBias
    desc.MaximumAnisotropy <- maximumAnisotropy
    desc.ComparisonFunction <- comparisonFunction
    desc.BorderColor <- borderColor
    desc.MinimumLod <- minimumLod
    desc.MaximumLod <- maximumLod
    desc
  let shaderResourceViewDescription format dimension buffer texture1D texture1DArray texture2D texture2DArray texture2DMS texture2DMSArray texture3D textureCube textureCubeArray bufferEx =
    let mutable desc = new ShaderResourceViewDescription()
    desc.Format <- format
    desc.Dimension <- dimension
    desc.Buffer <- buffer
    desc.Texture1D <- texture1D
    desc.Texture1DArray <- texture1DArray
    desc.Texture2D <- texture2D
    desc.Texture2DArray <- texture2DArray
    desc.Texture2DMS <- texture2DMS
    desc.Texture2DMSArray <- texture2DMSArray
    desc.Texture3D <- texture3D
    desc.TextureCube <- textureCube
    desc.TextureCubeArray <- textureCubeArray
    desc.BufferEx <- bufferEx
    desc
  let shaderResourceViewDescription1 format dimension buffer texture1D texture1DArray texture2D texture2DArray texture2DMS texture2DMSArray texture3D textureCube textureCubeArray bufferEx =
    let mutable desc = new ShaderResourceViewDescription1()
    desc.Format <- format
    desc.Dimension <- dimension
    desc.Buffer <- buffer
    desc.Texture1D <- texture1D
    desc.Texture1DArray <- texture1DArray
    desc.Texture2D <- texture2D
    desc.Texture2DArray <- texture2DArray
    desc.Texture2DMS <- texture2DMS
    desc.Texture2DMSArray <- texture2DMSArray
    desc.Texture3D <- texture3D
    desc.TextureCube <- textureCube
    desc.TextureCubeArray <- textureCubeArray
    desc.BufferEx <- bufferEx
    desc
  let streamOutputElement stream semanticName semanticIndex startComponent componentCount outputSlot =
    let mutable desc = new StreamOutputElement()
    desc.Stream <- stream
    desc.SemanticName <- semanticName
    desc.SemanticIndex <- semanticIndex
    desc.StartComponent <- startComponent
    desc.ComponentCount <- componentCount
    desc.OutputSlot <- outputSlot
    desc
  let streamOutputStatistics numPrimitivesWritten primitivesStorageNeeded =
    let mutable desc = new StreamOutputStatistics()
    desc.NumPrimitivesWritten <- numPrimitivesWritten
    desc.PrimitivesStorageNeeded <- primitivesStorageNeeded
    desc
  let subResourceTiling widthInTiles heightInTiles depthInTiles startTileIndexInOverallResource =
    let mutable desc = new SubResourceTiling()
    desc.WidthInTiles <- widthInTiles
    desc.HeightInTiles <- heightInTiles
    desc.DepthInTiles <- depthInTiles
    desc.StartTileIndexInOverallResource <- startTileIndexInOverallResource
    desc
  let texture1DDescription width mipLevels arraySize format usage bindFlags cpuAccessFlags optionFlags =
    let mutable desc = new Texture1DDescription()
    desc.Width <- width
    desc.MipLevels <- mipLevels
    desc.ArraySize <- arraySize
    desc.Format <- format
    desc.Usage <- usage
    desc.BindFlags <- bindFlags
    desc.CpuAccessFlags <- cpuAccessFlags
    desc.OptionFlags <- optionFlags
    desc
  let texture2DArrayVpov mipSlice firstArraySlice arraySize =
    let mutable desc = new Texture2DArrayVpov()
    desc.MipSlice <- mipSlice
    desc.FirstArraySlice <- firstArraySlice
    desc.ArraySize <- arraySize
    desc
  let texture2DDescription width height mipLevels arraySize format sampleDescription usage bindFlags cpuAccessFlags optionFlags =
    let mutable desc = new Texture2DDescription()
    desc.Width <- width
    desc.Height <- height
    desc.MipLevels <- mipLevels
    desc.ArraySize <- arraySize
    desc.Format <- format
    desc.SampleDescription <- sampleDescription
    desc.Usage <- usage
    desc.BindFlags <- bindFlags
    desc.CpuAccessFlags <- cpuAccessFlags
    desc.OptionFlags <- optionFlags
    desc
  let texture2DDescription1 width height mipLevels arraySize format sampleDescription usage bindFlags cpuAccessFlags optionFlags textureLayout =
    let mutable desc = new Texture2DDescription1()
    desc.Width <- width
    desc.Height <- height
    desc.MipLevels <- mipLevels
    desc.ArraySize <- arraySize
    desc.Format <- format
    desc.SampleDescription <- sampleDescription
    desc.Usage <- usage
    desc.BindFlags <- bindFlags
    desc.CpuAccessFlags <- cpuAccessFlags
    desc.OptionFlags <- optionFlags
    desc.TextureLayout <- textureLayout
    desc
  let texture2DVdov arraySlice =
    let mutable desc = new Texture2DVdov()
    desc.ArraySlice <- arraySlice
    desc
  let texture2DVpiv mipSlice arraySlice =
    let mutable desc = new Texture2DVpiv()
    desc.MipSlice <- mipSlice
    desc.ArraySlice <- arraySlice
    desc
  let texture2DVpov mipSlice =
    let mutable desc = new Texture2DVpov()
    desc.MipSlice <- mipSlice
    desc
  let texture3DDescription width height depth mipLevels format usage bindFlags cpuAccessFlags optionFlags =
    let mutable desc = new Texture3DDescription()
    desc.Width <- width
    desc.Height <- height
    desc.Depth <- depth
    desc.MipLevels <- mipLevels
    desc.Format <- format
    desc.Usage <- usage
    desc.BindFlags <- bindFlags
    desc.CpuAccessFlags <- cpuAccessFlags
    desc.OptionFlags <- optionFlags
    desc
  let texture3DDescription1 width height depth mipLevels format usage bindFlags cpuAccessFlags optionFlags textureLayout =
    let mutable desc = new Texture3DDescription1()
    desc.Width <- width
    desc.Height <- height
    desc.Depth <- depth
    desc.MipLevels <- mipLevels
    desc.Format <- format
    desc.Usage <- usage
    desc.BindFlags <- bindFlags
    desc.CpuAccessFlags <- cpuAccessFlags
    desc.OptionFlags <- optionFlags
    desc.TextureLayout <- textureLayout
    desc
  let tiledResourceCoordinate x y z subresource =
    let mutable desc = new TiledResourceCoordinate()
    desc.X <- x
    desc.Y <- y
    desc.Z <- z
    desc.Subresource <- subresource
    desc
  let tileRegionSize tileCount bUseBox width height depth =
    let mutable desc = new TileRegionSize()
    desc.TileCount <- tileCount
    desc.BUseBox <- bUseBox
    desc.Width <- width
    desc.Height <- height
    desc.Depth <- depth
    desc
  let tileShape widthInTexels heightInTexels depthInTexels =
    let mutable desc = new TileShape()
    desc.WidthInTexels <- widthInTexels
    desc.HeightInTexels <- heightInTexels
    desc.DepthInTexels <- depthInTexels
    desc
  let unorderedAccessViewDescription format dimension buffer texture1D texture1DArray texture2D texture2DArray texture3D =
    let mutable desc = new UnorderedAccessViewDescription()
    desc.Format <- format
    desc.Dimension <- dimension
    desc.Buffer <- buffer
    desc.Texture1D <- texture1D
    desc.Texture1DArray <- texture1DArray
    desc.Texture2D <- texture2D
    desc.Texture2DArray <- texture2DArray
    desc.Texture3D <- texture3D
    desc
  let unorderedAccessViewDescription1 format dimension buffer texture1D texture1DArray texture2D texture2DArray texture3D =
    let mutable desc = new UnorderedAccessViewDescription1()
    desc.Format <- format
    desc.Dimension <- dimension
    desc.Buffer <- buffer
    desc.Texture1D <- texture1D
    desc.Texture1DArray <- texture1DArray
    desc.Texture2D <- texture2D
    desc.Texture2DArray <- texture2DArray
    desc.Texture3D <- texture3D
    desc
  let videoColor yCbCr rgba =
    let mutable desc = new VideoColor()
    desc.YCbCr <- yCbCr
    desc.Rgba <- rgba
    desc
  let videoColorRgba r g b a =
    let mutable desc = new VideoColorRgba()
    desc.R <- r
    desc.G <- g
    desc.B <- b
    desc.A <- a
    desc
  let videoColorYCbCrA y cb cr a =
    let mutable desc = new VideoColorYCbCrA()
    desc.Y <- y
    desc.Cb <- cb
    desc.Cr <- cr
    desc.A <- a
    desc
  let videoContentProtectionCaps caps keyExchangeTypeCount blockAlignmentSize protectedMemorySize =
    let mutable desc = new VideoContentProtectionCaps()
    desc.Caps <- caps
    desc.KeyExchangeTypeCount <- keyExchangeTypeCount
    desc.BlockAlignmentSize <- blockAlignmentSize
    desc.ProtectedMemorySize <- protectedMemorySize
    desc
  let videoDecoderBeginFrameCryptoSession pCryptoSession blobSize pBlob pKeyInfoId privateDataSize pPrivateData =
    let mutable desc = new VideoDecoderBeginFrameCryptoSession()
    desc.PCryptoSession <- pCryptoSession
    desc.BlobSize <- blobSize
    desc.PBlob <- pBlob
    desc.PKeyInfoId <- pKeyInfoId
    desc.PrivateDataSize <- privateDataSize
    desc.PPrivateData <- pPrivateData
    desc
  let videoDecoderBufferDescription bufferType bufferIndex dataOffset dataSize firstMBaddress numMBsInBuffer width height stride reservedBits pIV iVSize partialEncryption encryptedBlockInfo =
    let mutable desc = new VideoDecoderBufferDescription()
    desc.BufferType <- bufferType
    desc.BufferIndex <- bufferIndex
    desc.DataOffset <- dataOffset
    desc.DataSize <- dataSize
    desc.FirstMBaddress <- firstMBaddress
    desc.NumMBsInBuffer <- numMBsInBuffer
    desc.Width <- width
    desc.Height <- height
    desc.Stride <- stride
    desc.ReservedBits <- reservedBits
    desc.PIV <- pIV
    desc.IVSize <- iVSize
    desc.PartialEncryption <- partialEncryption
    desc.EncryptedBlockInfo <- encryptedBlockInfo
    desc
  let videoDecoderBufferDescription1 bufferType dataOffset dataSize pIV iVSize pSubSampleMappingBlock subSampleMappingCount =
    let mutable desc = new VideoDecoderBufferDescription1()
    desc.BufferType <- bufferType
    desc.DataOffset <- dataOffset
    desc.DataSize <- dataSize
    desc.PIV <- pIV
    desc.IVSize <- iVSize
    desc.PSubSampleMappingBlock <- pSubSampleMappingBlock
    desc.SubSampleMappingCount <- subSampleMappingCount
    desc
  let videoDecoderConfig guidConfigBitstreamEncryption guidConfigMBcontrolEncryption guidConfigResidDiffEncryption configBitstreamRaw configMBcontrolRasterOrder configResidDiffHost configSpatialResid8 configResid8Subtraction configSpatialHost8or9Clipping configSpatialResidInterleaved configIntraResidUnsigned configResidDiffAccelerator configHostInverseScan configSpecificIDCT config4GroupedCoefs configMinRenderTargetBuffCount configDecoderSpecific =
    let mutable desc = new VideoDecoderConfig()
    desc.GuidConfigBitstreamEncryption <- guidConfigBitstreamEncryption
    desc.GuidConfigMBcontrolEncryption <- guidConfigMBcontrolEncryption
    desc.GuidConfigResidDiffEncryption <- guidConfigResidDiffEncryption
    desc.ConfigBitstreamRaw <- configBitstreamRaw
    desc.ConfigMBcontrolRasterOrder <- configMBcontrolRasterOrder
    desc.ConfigResidDiffHost <- configResidDiffHost
    desc.ConfigSpatialResid8 <- configSpatialResid8
    desc.ConfigResid8Subtraction <- configResid8Subtraction
    desc.ConfigSpatialHost8or9Clipping <- configSpatialHost8or9Clipping
    desc.ConfigSpatialResidInterleaved <- configSpatialResidInterleaved
    desc.ConfigIntraResidUnsigned <- configIntraResidUnsigned
    desc.ConfigResidDiffAccelerator <- configResidDiffAccelerator
    desc.ConfigHostInverseScan <- configHostInverseScan
    desc.ConfigSpecificIDCT <- configSpecificIDCT
    desc.Config4GroupedCoefs <- config4GroupedCoefs
    desc.ConfigMinRenderTargetBuffCount <- configMinRenderTargetBuffCount
    desc.ConfigDecoderSpecific <- configDecoderSpecific
    desc
  let videoDecoderDescription guid sampleWidth sampleHeight outputFormat =
    let mutable desc = new VideoDecoderDescription()
    desc.Guid <- guid
    desc.SampleWidth <- sampleWidth
    desc.SampleHeight <- sampleHeight
    desc.OutputFormat <- outputFormat
    desc
  let videoDecoderExtension func pPrivateInputData privateInputDataSize pPrivateOutputData privateOutputDataSize resourceCount ppResourceList =
    let mutable desc = new VideoDecoderExtension()
    desc.Function <- func
    desc.PPrivateInputData <- pPrivateInputData
    desc.PrivateInputDataSize <- privateInputDataSize
    desc.PPrivateOutputData <- pPrivateOutputData
    desc.PrivateOutputDataSize <- privateOutputDataSize
    desc.ResourceCount <- resourceCount
    desc.PpResourceList <- ppResourceList
    desc
  let videoDecoderOutputViewDescription decodeProfile dimension texture2D =
    let mutable desc = new VideoDecoderOutputViewDescription()
    desc.DecodeProfile <- decodeProfile
    desc.Dimension <- dimension
    desc.Texture2D <- texture2D
    desc
  let videoDecoderSubSampleMappingBlock clearSize encryptedSize =
    let mutable desc = new VideoDecoderSubSampleMappingBlock()
    desc.ClearSize <- clearSize
    desc.EncryptedSize <- encryptedSize
    desc
  let videoProcessorCaps deviceCaps featureCaps filterCaps inputFormatCaps autoStreamCaps stereoCaps rateConversionCapsCount maxInputStreams maxStreamStates =
    let mutable desc = new VideoProcessorCaps()
    desc.DeviceCaps <- deviceCaps
    desc.FeatureCaps <- featureCaps
    desc.FilterCaps <- filterCaps
    desc.InputFormatCaps <- inputFormatCaps
    desc.AutoStreamCaps <- autoStreamCaps
    desc.StereoCaps <- stereoCaps
    desc.RateConversionCapsCount <- rateConversionCapsCount
    desc.MaxInputStreams <- maxInputStreams
    desc.MaxStreamStates <- maxStreamStates
    desc
  let videoProcessorColorSpace =
    let mutable desc = new VideoProcessorColorSpace()
    desc
  let videoProcessorContentDescription inputFrameFormat inputFrameRate inputWidth inputHeight outputFrameRate outputWidth outputHeight usage =
    let mutable desc = new VideoProcessorContentDescription()
    desc.InputFrameFormat <- inputFrameFormat
    desc.InputFrameRate <- inputFrameRate
    desc.InputWidth <- inputWidth
    desc.InputHeight <- inputHeight
    desc.OutputFrameRate <- outputFrameRate
    desc.OutputWidth <- outputWidth
    desc.OutputHeight <- outputHeight
    desc.Usage <- usage
    desc
  let videoProcessorCustomRate customRate outputFrames inputInterlaced inputFramesOrFields =
    let mutable desc = new VideoProcessorCustomRate()
    desc.CustomRate <- customRate
    desc.OutputFrames <- outputFrames
    desc.InputInterlaced <- inputInterlaced
    desc.InputFramesOrFields <- inputFramesOrFields
    desc
  let videoProcessorFilterRange minimum maximum def multiplier =
    let mutable desc = new VideoProcessorFilterRange()
    desc.Minimum <- minimum
    desc.Maximum <- maximum
    desc.Default <- def
    desc.Multiplier <- multiplier
    desc
  let videoProcessorInputViewDescription fourCC dimension texture2D =
    let mutable desc = new VideoProcessorInputViewDescription()
    desc.FourCC <- fourCC
    desc.Dimension <- dimension
    desc.Texture2D <- texture2D
    desc
  let videoProcessorOutputViewDescription dimension texture2D texture2DArray =
    let mutable desc = new VideoProcessorOutputViewDescription()
    desc.Dimension <- dimension
    desc.Texture2D <- texture2D
    desc.Texture2DArray <- texture2DArray
    desc
  let videoProcessorRateConversionCaps pastFrames futureFrames processorCaps iTelecineCaps customRateCount =
    let mutable desc = new VideoProcessorRateConversionCaps()
    desc.PastFrames <- pastFrames
    desc.FutureFrames <- futureFrames
    desc.ProcessorCaps <- processorCaps
    desc.ITelecineCaps <- iTelecineCaps
    desc.CustomRateCount <- customRateCount
    desc
  let videoProcessorStream enable outputIndex inputFrameOrField pastFrames futureFrames ppPastSurfaces pInputSurface ppFutureSurfaces ppPastSurfacesRight pInputSurfaceRight ppFutureSurfacesRight =
    let mutable desc = new VideoProcessorStream()
    desc.Enable <- enable
    desc.OutputIndex <- outputIndex
    desc.InputFrameOrField <- inputFrameOrField
    desc.PastFrames <- pastFrames
    desc.FutureFrames <- futureFrames
    desc.PpPastSurfaces <- ppPastSurfaces
    desc.PInputSurface <- pInputSurface
    desc.PpFutureSurfaces <- ppFutureSurfaces
    desc.PpPastSurfacesRight <- ppPastSurfacesRight
    desc.PInputSurfaceRight <- pInputSurfaceRight
    desc.PpFutureSurfacesRight <- ppFutureSurfacesRight
    desc
  let videoProcessorStreamBehaviorHint enable width height format =
    let mutable desc = new VideoProcessorStreamBehaviorHint()
    desc.Enable <- enable
    desc.Width <- width
    desc.Height <- height
    desc.Format <- format
    desc
  let videoSampleDescription width height format colorSpace =
    let mutable desc = new VideoSampleDescription()
    desc.Width <- width
    desc.Height <- height
    desc.Format <- format
    desc.ColorSpace <- colorSpace
    desc
  let streamOutputBufferBinding =
    let mutable desc = new StreamOutputBufferBinding()
    desc
  let vertexBufferBinding =
    let mutable desc = new VertexBufferBinding()
    desc
