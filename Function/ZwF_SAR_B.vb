variables: AfStep(0.02), AfLimit(0.2) ; //抛物线参数
inputs: var0(NumericRef); //抛物线值
inputs: var2(NumericRef); //抛物线多空标识（1--》多头，抛物线在k线下方，-1--》空头，抛物线在k线上方）
inputs: var3(NumericRef); //抛物线多空转换点标识（1--》空转多，-1--》多转空，0--》非转换点）
variables:var1(0), var4("") ; //抛物线其他值

variables: SAR_high_Q( High ); //上抛物趋势线前起点值
variables: SAR_high_H( High );//上抛物趋势线后起点值
variables: SAR_high_I( High );//上抛物趋势线后起点值的差
variables: SAR_high_X( 0 ); //上抛物线趋势斜率
variables: SAR_high_N_Q( 1 );//上抛物趋势线前起点的barnumber
variables: SAR_high_N_H( 1 );//上抛物趋势线后起点的barnumber
variables: SAR_high_N_I( 1 );//上抛物趋势线前后起点的间隔bar数
variables: upper_rail_num(1);//上抛物趋势线编号
variables: IntraBarPersist SAR_up_H( High );//向上突破点Bar的最高价
variables: SAR_up_L( Low );//向上突破点Bar的最低价

variables: SAR_low_Q( Low );//下抛物趋势线前起点值
variables: SAR_low_H( Low );//下抛物趋势线后起点值
variables: SAR_low_I( Low );//下抛物趋势线后起点值的差
variables: SAR_low_X( 0 );//下抛物趋势线斜率
variables: SAR_low_N_Q( 1 );//下抛物趋势线前起点的barnumber
variables: SAR_low_N_H( 1 );//下抛物趋势线后起点的barnumber
variables: SAR_low_N_I( 1 );//下抛物趋势线前后起点的间隔bar数
variables: lower_rail_num(1);//下抛物趋势线编号
variables: SAR_down_H( High );//向下突破点Bar的最高价
variables: IntraBarPersist SAR_down_L( Low );//向下突破点Bar的最低价

inputs: DKKZ(NumericRef);//多空控制（1-》上升通道，2-》下降通道，3-》三角通道，？1-》收敛，？2-》发散）
inputs: dKcPrice(NumericRef);//多头开仓点位
inputs: dZsPrice(NumericRef);//多头止损点位
inputs: kKcPrice(NumericRef);//空头开仓点位
inputs: kZsPrice(NumericRef);//空头止损点位
inputs: KcPrice(NumericRef);//开仓点位
inputs: ZsPrice(NumericRef);//止损点位

if CurrentBar = 1 then
	begin
	dKcPrice = High;	
	dZsPrice = Low;
	kKcPrice = Low;
	kZsPrice = High;
	KcPrice = Open;
	ZsPrice = Open;
	DKKZ = 0;
	end ;

Value1 = ParabolicSAR( AfStep, AfLimit, var0, var1, var2, var3 ) ; //抛物线指标
condition1 =  SAR_high_N_Q <> SAR_high_N_H and SAR_low_N_Q <> SAR_low_N_H; //抛物线起点已经历三次突破，初始化用

if var2 <> var2[1] then //抛物线突破点（转换点）
	begin
	//Text_New_BN (BarNumber, var0,"+"); //绘制抛物线突破点（转换点）
	DKKZ = 0;
	if var3 > 0 then  //向上突破的
		begin
		SAR_low_N_Q = SAR_low_N_H;
		SAR_low_N_H = BarNumber; //获取向上突破点位置的BarNumber
		SAR_low_N_I = SAR_low_N_H - SAR_low_N_Q; //获取前后向上突破点之间Bar的数量
		SAR_low_Q = SAR_low_H;
		SAR_low_H = var0;//获取向上突破点抛物线的值
		SAR_low_I = SAR_low_H - SAR_low_Q; //获取前后向上突破点之差
		if SAR_low_N_I>0 then
			SAR_low_X = SAR_low_I/SAR_low_N_I; //下趋势线斜率
		condition2 = SAR_low_X > 0; //下趋势线斜率向上
		//upper_rail_num = tl_new_bn(SAR_low_N_Q,SAR_low_Q ,SAR_low_N_H ,SAR_low_H ); //以下抛物线前后起始点画支撑线
		//Value2 = TL_SetBegin_BN(upper_rail_num,SAR_low_N_H ,SAR_low_H ); //以下抛物线前后起始点画支撑线
		//TL_SetExtRight(upper_rail_num,True); //向右延申
		SAR_up_H = High; //获取向上突破点Bar的最高价
		SAR_up_L = Low; //获取向上突破点Bar的最低价
		
		dKcPrice = SAR_up_H + 1 point * MinMove; //开仓线-多头
		dZsPrice = SAR_low_H - 1 point * MinMove; //止损线-多头
		KcPrice = dKcPrice;
		ZsPrice = dZsPrice;
		//Text_New_BN (BarNumber, dKcPrice,"---dK----C---") ; //开仓线
		//Text_New_BN (BarNumber, dZsPrice,"---dZ----S---") ; //止损线
		if condition1 then
			begin
			if SAR_low_X > 0 and SAR_high_X > 0 then //向上突破的-上升通道（上下趋势线斜率均为正）
				begin
				DKKZ = 1; //多空控制-》向上突破的
				if SAR_low_X > SAR_high_X then //向上突破的--上升收敛通道（上下趋势线斜率均为正，下趋势线斜率大于上趋势线斜率）
					begin
					DKKZ = 11; //多空控制-》向上突破-》上升通道-》收敛
					end
				else //上升发散通道（上下趋势线斜率均为正，下趋势线斜率小于趋势线斜率）
					begin
					DKKZ = 12; //多空控制-》向上突破-》上升通道-》发散
					end;
				end
			else if SAR_low_X < 0 and SAR_high_X < 0 then //下降通道（上下趋势线斜率均为负）
				begin
				DKKZ = 2; //多空控制-》向上突破-》下降通道
				if SAR_low_X > SAR_high_X then //下降收敛通道（上下趋势线斜率均为负，下趋势线斜率大于上趋势线斜率）
					begin
					DKKZ = 21; //多空控制-》向上突破-》下降通道-》收敛
					end
				else //下降发散通道（上下趋势线斜率均为负，下趋势线斜率小于上趋势线斜率）
					begin
					DKKZ = 22; //多空控制-》向上突破-》下降通道-》发散
					end;
				end
			else
				begin
				if SAR_low_X > 0 and SAR_high_X < 0 then //向上突破的三角通道（上趋势线斜率为负，下趋势线斜率为正）
					begin
					DKKZ = 31; //多空控制-》向上突破-》三角形态
					end
				else
					begin
					DKKZ = 32; //多空控制-》向上突破-》发散形态
					end;
				end;
			end;
		end
	else if var3 < 0 then //向下突破
		begin		
		SAR_high_N_Q = SAR_high_N_H;
		SAR_high_N_H = BarNumber;//获取向下突破点位置的BarNumber
		SAR_high_N_I = SAR_high_N_H - SAR_high_N_Q; //获取前后向下突破点之间Bar的数量
		SAR_high_Q = SAR_high_H;
		SAR_high_H = var0;//获取向下突破点抛物线的值
		SAR_high_I = SAR_high_H - SAR_high_Q;
		if SAR_high_N_I>0 then
			SAR_high_X = SAR_high_I/SAR_high_N_I;//上趋势线斜率
		condition3 = SAR_high_X > 0; //上趋势线斜率向上
		//lower_rail_num = tl_new_bn(SAR_high_N_Q,SAR_high_Q,SAR_high_N_H ,SAR_high_H );//以上抛物线前后起始点画压力线
		//Value2 = TL_SetBegin_BN(lower_rail_num,SAR_high_N_H ,SAR_high_H ); 
		//TL_SetExtRight(lower_rail_num,True); //向右延申
		SAR_down_H = High;//获取向下突破点Bar的最高价
		SAR_down_L = Low;//获取向下突破点Bar的最低价
		
		kKcPrice = SAR_down_L - 1 point * MinMove;//开仓线-空头
		kZsPrice = SAR_high_H + 1 point * MinMove; //止损线-空头
		KcPrice = kKcPrice;
		ZsPrice = kZsPrice;	
		//Text_New_BN (BarNumber, kKcPrice,"---kK----C---") ; //开仓线
		//Text_New_BN (BarNumber, kZsPrice,"---kZ----S---") ; //止损线
					
		if condition1 then
			begin			
			if SAR_low_X > 0 and SAR_high_X > 0 then //上升通道（上下趋势线斜率均为正）
				begin
				DKKZ = -1; //多空控制-》向下突破-》上升通道
				if SAR_low_X > SAR_high_X then //上升收敛通道（上下趋势线斜率均为正，下趋势线斜率大于上趋势线斜率）
					begin
					DKKZ = -11; //多空控制-》上升通道-》上升通道-》收敛
					end
				else //上升发散通道
					begin
					DKKZ = -12; //多空控制-》向下突破-》上升通道-》发散
					end;
				end
			else if SAR_low_X < 0 and SAR_high_X < 0 then //下降通道（上下趋势线斜率均为负）
				begin
				DKKZ = -2; //多空控制-》向下突破-》下降通道
				if SAR_low_X > SAR_high_X then //下降收敛通道（上下趋势线斜率均为负，下趋势线斜率大于上趋势线斜率）
					begin
					DKKZ = -21; //多空控制-》向下突破-》下降通道-》收敛			
					end
				else //下降发散通道（上下趋势线斜率均为负，下趋势线斜率小于上趋势线斜率）
					begin
					DKKZ = -22; //多空控制-》向下突破-》下降通道-》发散
					end;
				end
			else
				begin
				if SAR_low_X > 0 and SAR_high_X < 0 then //三角通道（上下趋势线斜率均为负，下趋势线斜率为正）
					begin
					DKKZ = -31; //多空控制-》向下突破-》三角形态
					end
				else
					begin
					DKKZ = -32; //多空控制-》向下突破-》发散形态
					end;
				end;
			end;
		end;
	end;