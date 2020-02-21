inputs: InitCap(NumericSimple);//初始资金
inputs: DKKZ(NumericSeries);//多空控制
inputs: dKcPrice(NumericSeries);//多头开仓点位
inputs: dZsPrice(NumericSeries);//多头止损点位
inputs: kKcPrice(NumericSeries);//空头开仓点位
inputs: kZsPrice(NumericSeries);//空头止损点位

inputs: DopenWarehousePosition(NumericRef);//开仓数量-多头
inputs: KopenWarehousePosition(NumericRef);//开仓数量-多头
inputs: dZsPriceT(NumericRef);//止损点位-多头
inputs: kZsPriceT(NumericRef);//止损点位-空头

variables: DstopLossPoints( 0 );//止损点差
variables: KstopLossPoints( 0 );//止损点差
variables: pointMoveVaule( 0 );//一跳的价值
variables: pointMovePoint( 0 );//一跳的点数
variables: maxStopPoints( 0 );//最大止损点差
//value10 = InitialCapital*0.01;
value10 = InitCap*0.01;
//value10 = 30000*0.01;
if DKKZ > 0 then
	begin
	DstopLossPoints = AbsValue(dKcPrice - dZsPrice);//止损点差-多头
	if DstopLossPoints>0 and BigPointvalue>0 then
		begin
		DopenWarehousePosition = IntPortion(value10 / (DstopLossPoints*BigPointvalue));
		if DopenWarehousePosition < 1 then
			begin
			DopenWarehousePosition = 1;
			pointMoveVaule = PointValue * MinMove; //一跳的价值
			pointMovePoint = 1 point * MinMove; //一跳的点数
			maxStopPoints = value10 / pointMoveVaule * pointMovePoint; //最大止损点差
			dZsPriceT = dKcPrice - maxStopPoints;
			end
		else
			dZsPriceT = dZsPrice;
		end;
	end
else if DKKZ < 0 then
	begin
	KstopLossPoints = AbsValue(kZsPrice - kKcPrice);//止损点差-空头
	if KstopLossPoints>0 and BigPointvalue>0 then
		begin
		KopenWarehousePosition = IntPortion(value10/(KstopLossPoints*BigPointvalue));
		if KopenWarehousePosition < 1 then
			begin
			KopenWarehousePosition = 1;
			pointMoveVaule = PointValue * MinMove; //一跳的价值
			pointMovePoint = 1 point * MinMove; //一跳的点数
			maxStopPoints = value10 / pointMoveVaule * pointMovePoint; //最大止损点差
			KZsPriceT = kKcPrice + maxStopPoints;
			end
		else
			KZsPriceT = kZsPrice;
		end;
	end
else
	begin
	DopenWarehousePosition = 0;
	KopenWarehousePosition = 0;
	DZsPriceT = Low;
	KZsPriceT = High;	
	end;
