[IntrabarOrderGeneration = False]
//Zw-T-BB03-dev，布林通道上下轨交叉点的高低点

////---布林指标值------------////
inputs:	BBLength(50); //布林指标-长度参数
//---布林指标返回值------------
variables: MAbb(C); //布林指标-参考均线
variables: BollUp(H); //布林指-标上轨
variables: BollDn(L); //布林指-标下轨
////---布林交叉点HL指标------------////
variables: BB_Cu_H(H); //突破布林上轨K线最高点
variables: BB_Cu_Ma(L); //突破布林上轨布林均线值
variables: BB_Cd_L(L); //突破布林下轨K线最低点
variables: BB_Cd_Ma(H); //突破布林下轨布林均线值
variables: DKBoll1(0); //多空标志-布林突破（突破上轨1，突破下轨-1）
variables: DKBoll2(0); //多空标志-布林突破（突破上轨点1，突破下轨点-1，其余0）
value1 = Zw_F_BB_CsHL(Close,BBLength,2,MAbb,BollUp,BollDn,BB_Cu_H,BB_Cu_Ma,BB_Cd_L,BB_Cd_Ma,DKBoll1,DKBoll2);
	
//--------------开仓量计算-------------
Input: InitCaptal(30000);// 初始资金
Input: MaximumLossRatio(0.01);// 最大损失比例
variables: StopLossPoint(0);//止损点差
variables: MaximumLossMargin(0);//最大损失额
variables: KCS(0);//开仓数
MaximumLossMargin = InitCaptal * MaximumLossRatio;
if DKBoll2 > 0 then
	StopLossPoint = BB_Cu_H - BB_Cu_Ma
else if DKBoll2 < 0 then
	StopLossPoint = BB_Cd_Ma - BB_Cd_L;
value2 = Zw_F_ZsP_1(MaximumLossMargin,StopLossPoint,KCS);
//--------------止损线计算-------------
variables: MaxStopLossPoint(0);//止损点差
variables: ZsLine(C);//止损线
if KCS > 0 and BigPointvalue > 0 then
	MaxStopLossPoint = MaximumLossMargin/KCS/BigPointvalue;
	if MaxStopLossPoint > StopLossPoint then
		MaxStopLossPoint = StopLossPoint;
if DKBoll1 > 0 then
	ZsLine = BB_Cu_H - MaxStopLossPoint
else if DKBoll1 < 0 then
	ZsLine = BB_Cd_L + MaxStopLossPoint;

//--------------开仓-------------//
variables: DKKZ(0); //多空控制
variables: MP(0); //持仓方向
variables: DkcLine(H); //多头开仓线
variables: KkcLine(L); //空头开仓线
variables: ZsLineT(C); //止损线
MP = marketposition;
//--------------多空控制阀-------------
if MP <> 1 and DKBoll2 > 0 then
	DKKZ = 1
else if MP <> -1 and DKBoll2 < 0 then
	DKKZ = -1
else if MP <> MP[1] then
	DKKZ = 0;

if KCS > 0 and DKKZ <> 0 then
	begin
	DkcLine = BB_Cu_H + 1 point * MinMove;
	KkcLine = BB_Cd_L - 1 point * MinMove;
	condition1 = HighD(0) <> LowD(0); //非涨停
	//condition2 = BollUp[1] > BollUp[2] and H < var1D and MA1[1] > MA1[2]; //D
	//condition3 = BollDn[1] < BollDn[2] and L > var1K and MA1[1] < MA1[2]; //K
	condition2 = True;//var0D;// and BollUp > BollUp[1] and MA1 > MA1[1]; //D
	condition3 = True;//var0K;// and BollDn < BollDn[1] and MA1 < MA1[1];  //K
	if condition1 then
		begin
		if condition2 then//
			begin
			buy("dKc-BB") KCS shares next bar at DkcLine stop;
			end;
		if condition3 then//
			begin
			sellshort("kKc-BB") KCS shares next bar at KkcLine stop;
			end;
		end;
	end ;
//----------------------平仓-------------------------------//
variables: cclineD(0); //平仓线-多头
variables: cclineK(0); //平仓线-空头

if MP <> MP[1] then
	ZsLineT = ZsLine;  //固定止损点
cclineD = BollDn[1] - 1 point * MinMove; //多头平仓线-布林下轨
cclineK = BollUp[1] + 1 point * MinMove; //空头平仓线-布林上轨

if MP > 0 then
	begin
	{if Close crosses below MAbb then
		begin
		sell ("PD") all shares next bar at market;
		end;}
	//固定止损
	sell("dZs-TT") all shares next bar at ZsLineT stop;
	//动态止损
	sell("dZs-QX") all shares next bar at BB_Cu_Ma - 1 point * MinMove stop;
	
	sell ("dZs-BB") all shares next bar at cclineD stop;
	end;
if MP < 0 then
	begin
	{if Close crosses above MAbb then
		begin
		buytocover ("PK") all shares next bar at market;
		end;}
	//固定止损
	buytocover("kZs-TT") all shares next bar at ZsLineT stop;
	//动态止损
	buytocover("kZs-QX") all shares next bar at BB_Cd_Ma + 1 point * MinMove stop;
	buytocover ("kZs-BB") all shares next bar at cclineK stop;
	end;
