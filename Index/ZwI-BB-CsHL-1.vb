//Zw-I-BB-CsHL-1，布林通道上下轨交叉点的高低点
////---布林指标值------------////
inputs:	BBLength(50); //布林指标-长度参数
//---布林指标返回值------------
variables: MAbb(Close); //布林指标-参考均线
variables: BollUp(High); //布林指-标上轨
variables: BollDn(Low); //布林指-标下轨

////---布林交叉点HL指标------------////
variables: BB_C_P(Close); //突破布林上下轨K线最高低点
variables: BB_C_Ma(Close); //突破布林上下轨布林均线值
variables: DKBoll1(0); //多空标志-布林突破（突破上轨1，突破下轨-1）
variables: DKBoll2(0); //多空标志-布林突破（突破上轨点1，突破下轨点-1，其余0）

value1 = ZwF_BB_CsHL1(Close,BBLength,2,MAbb,BollUp,BollDn,BB_C_P,BB_C_Ma,DKBoll1,DKBoll2);

//--------------开仓量计算-------------
Input: InitCaptal(30000);// 初始资金
Input: MaximumLossRatio(0.01);// 最大损失比例
variables: StopLossPoint(0);//止损点差
variables: MaximumLossMargin(0);//最大损失额
variables: KCS(0);//开仓数
MaximumLossMargin = InitCaptal * MaximumLossRatio;
StopLossPoint = absvalue(BB_C_P - BB_C_Ma);

value2 = ZwF_ZsP1(MaximumLossMargin,StopLossPoint,KCS);
//--------------最大止损点差计算-------------
variables: MaxStopLossPoint(0);//止损点差
variables: ZsLine(Close);//止损线
if KCS > 0 and BigPointvalue > 0 then
	MaxStopLossPoint = MaximumLossMargin/KCS/BigPointvalue;
	if MaxStopLossPoint > StopLossPoint then
		MaxStopLossPoint = StopLossPoint;
if DKBoll1 > 0 then
	ZsLine = BB_C_P - MaxStopLossPoint
else if DKBoll1 < 0 then
	ZsLine = BB_C_P + MaxStopLossPoint;


plot1(MAbb,"MAbb");
plot2(BollUp,"BollUp");
plot3(BollDn,"BollDn");
plot4(BB_C_P,"BB_C_P");
plot5(BB_C_Ma,"BB_C_Ma");
plot6(ZsLine,"ZsLine");