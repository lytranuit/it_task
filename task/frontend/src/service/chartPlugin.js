const labelCenter = {
  id: "labelCenter",
  beforeDraw: (chart, args, options) => {
    if (!Array.isArray(options.labels)) {
      return console.error('"options.lables" must be array', options.lables);
    }

    const {
      ctx,
      chartArea: { width, height }
    } = chart;
    const context = ctx.canvas.getContext("2d");
    context.save();

    const labels = options.labels.slice();
    const totalFontSize = labels.reduce((x, y, index) => {
      let prevSize = x[index - 1] || 0;
      x.push(prevSize + y.font.size);
      return x;
    }, []);
    labels.forEach((label, index) => {
      let textPosition = totalFontSize[index] - label.font.size;
      const values = {
        text: label.text || "?",
        font: {
          size: label.font.size || 30,
          family: label.font.family || "sans-serif",
          color: label.font.color || "black",
          style: label.font.style || "normal",
          unit: label.font.unit || "px"
        }
      };
      switch (values.font.unit) {
        case "em":
          values.font.size = label.font.size * 0.05;
          textPosition *= 0.8;
          break;
        default:
          break;
      }

      context.font = `${values.font.style} ${values.font.size}${values.font.unit} ${values.font.family}`;
      context.textAlign = "center";
      context.fillStyle = values.font.color;
      context.fillText(values.text, width / 2, height / 2 + textPosition);
    });
    context.restore();
  }
}
export { labelCenter }