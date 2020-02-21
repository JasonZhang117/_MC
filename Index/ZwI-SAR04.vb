variables: var0(0); //抛物线值
variables: var2(0); //抛物线多空标识（1--》多头，抛物线在k线下方，-1--》空头，抛物线在k线上方）
variables: var3(0); //抛物线多空转换点标识（1--》空转多，-1--》多转空，0--》非转换点）

variables: DKKZ(0);//多空控制
variables: dKcPrice(High);//多头开仓点位
variables: dZsPrice(Low);//多头止损点位
variables: kKcPrice(Low);//空头开仓点位
variables: kZsPrice(High);//空头止损点位
variables: KcPrice(Open);//开仓点位
variables: ZsPrice(Open);//止损点位
//------------------指标计算------------------//
//------------------抛物线形态指标------------------//
value1 = ZwF_SAR2(var0,var2,var3,DKKZ,dKcPrice,dZsPrice,kKcPrice,kZsPrice,KcPrice,ZsPrice);

//------------------开仓量、真实止损线------------------//
inputs: InitCap(30000);
variables: dOPW( 0 );//开仓数-多头
variables: kOPW( 0 );//开仓数-空头
variables: dZsPriceT(Low);//止损点位(头寸函数返回)-多头
variables: kZsPriceT(High);//止损点位(头寸函数返回)-空头
value2 = ZwF_ZsP(InitCap,DKKZ,dKcPrice,dZsPrice,kKcPrice,kZsPrice,dOPW,kOPW,dZsPriceT,kZsPriceT);


plot1(var0,"SAR");

plot2(dKcPrice,"dKcPrice");
plot3(dZsPriceT,"dZsPriceT");
plot4(dZsPrice,"dZsPrice");
plot5(kKcPrice,"kKcPrice");
plot6(kZsPriceT,"kZsPriceT");
plot7(kZsPrice,"kZsPrice");

