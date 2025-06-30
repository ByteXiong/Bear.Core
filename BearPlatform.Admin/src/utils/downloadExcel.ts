import dayjs from "dayjs";

// 支持传入HTMLElement或元素ID
export default function downloadExcel(el: HTMLElement | string, name: string) {
  const targetEl = typeof el === 'string' 
    ? document.getElementById(el)
    : el;

  if (!targetEl) {
    console.error("无效的表格元素");
    return;
  }

  // 深拷贝避免污染原DOM
  const cloneNode = targetEl.cloneNode(true) as HTMLElement;
  removeElementsByClass(cloneNode, "onExcel");

  // 修正的Excel HTML模板
  const html = `<!DOCTYPE html>
<html xmlns:o="urn:schemas-microsoft-com:office:office" 
      xmlns:x="urn:schemas-microsoft-com:office:excel">
<head>
  <meta charset="UTF-8">
  <!--[if gte mso 9]>
  <xml>
    <x:ExcelWorkbook>
      <x:ExcelWorksheets>
        <x:ExcelWorksheet>
          <x:Name>${name}</x:Name>
          <x:WorksheetOptions>
            <x:DisplayGridlines/>
          </x:WorksheetOptions>
        </x:ExcelWorksheet>
      </x:ExcelWorksheets>
    </x:ExcelWorkbook>
  </xml>
  <![endif]-->
</head>
<body>${cloneNode.innerHTML}</body>
</html>`;

  const blob = new Blob([html], {
    type: "application/vnd.ms-excel;charset=UTF-8" // 修正MIME类型
  });

  const link = document.createElement("a");
  const url = URL.createObjectURL(blob);

  link.href = url;
  link.download = `${name}_${dayjs().format("YYYYMMDDHHmmss")}.xls`;
  document.body.appendChild(link);
  link.click();

  // 确保资源释放
  setTimeout(() => {
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
  }, 100);
}

// 优化后的DOM清理方法
function removeElementsByClass(root: HTMLElement, className: string) {
  const elements = root.getElementsByClassName(className);
  while (elements.length > 0) {
    elements[0].remove();
  }
  
  Array.from(root.children).forEach(child => {
    removeElementsByClass(child as HTMLElement, className);
  });
}
