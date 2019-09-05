# Mdate
移动端时间选择插件

```
//new Mdate("dateSelectorOne");
new Mdate("dateShowBtn", { //"dateShowBtn"为你点击触发Mdate的id，必填项
    acceptId: "dateput",//此项为你要显示选择后的日期的input写默认为上一行的"dateShowBtn"
    beginYear: "1950",//此项为Mdate的初始年份，不填写默认为2000
    beginMonth: "1",//此项为Mdate的初始月份，不填写默认为1
    beginDay: "1",//此项为Mdate的初始日期，不填写默认为1
    endYear: "2099",//此项为Mdate的结束年份，不填写默认为当年
    endMonth: "12",//此项为Mdate的结束月份，不填写默认为当月
    endDay: "31",//此项为Mdate的结束日期，不填写默认为当天
    format: "-"//此项为Mdate需要显示的格式，可填写"/"或"-"或".",不填写默认为年月日
})

```
![图片演示](https://github.com/wo2tanglili/Mdate/blob/master/demo.png)
![手机演示地址](https://github.com/wo2tanglili/Mdate/blob/master/1561456157.png)