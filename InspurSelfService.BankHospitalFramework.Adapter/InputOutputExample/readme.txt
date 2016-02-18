1.BankCallHis：银行调用His接口
说明：银行发起请求经由银医适配器调用His接口
根据His接口实际情况确定选用参数方式，直接在目录下相应xml文件中有序编写示例参数。
【参数方式】
（1）IHSSCallHis-XML.xml
<?xml version="1.0" encoding="utf-8" ?>
<root>
  <method name="" type="" summary="">
    <input></input>
    <output></output>
  </method>
</root>

（2）IHSSCallHis-SQLServer.xml
<?xml version="1.0" encoding="utf-8" ?>
<root>
  <method name="" type="" summary="">
    <input></input>
    <output></output>
  </method>
</root>

（3）IHSSCallHis-Oracle.xml
<?xml version="1.0" encoding="utf-8" ?>
<root>
  <method name="" type="" summary="">
    <input></input>
    <output></output>
  </method>
</root>

（3）IHSSCallHis-Json.xml
<?xml version="1.0" encoding="utf-8" ?>
<root>
  <method name="" type="" summary="">
    <input></input>
    <output></output>
  </method>
</root>

（4）IHSSCallHis-StringJoin.xml
<?xml version="1.0" encoding="utf-8" ?>
<root>
  <method name="" type="" summary="">
    <input></input>
    <output></output>
  </method>
</root>

（5）IHSSCallHis-XML.xml
<?xml version="1.0" encoding="utf-8" ?>
<root>
  <method name="" type="" summary="">
    <input></input>
    <output></output>
  </method>
</root>


2.HisCallBank：His调用His接口
说明：His发起请求经由银医适配器调用银行接口
根据银行接口实际情况确定选用参数方式，直接在目录下相应xml文件中有序编写示例参数。
【参数方式】
（1）IHSSCallHis-XML.xml
<?xml version="1.0" encoding="utf-8" ?>
<root>
  <method name="" type="" summary="">
    <input></input>
    <output></output>
  </method>
</root>

（2）IHSSCallHis-StringJoin.xml
<?xml version="1.0" encoding="utf-8" ?>
<root>
  <method name="" type="" summary="">
    <input></input>
    <output></output>
  </method>
</root>


3.IHSSCallBank：银医系统调用银行接口
说明：银医系统发起请求调用银行接口
【参数方式】

4.IHSSCallHis：银医系统调用His接口
说明：银医系统发起请求调用His接口
【参数方式】