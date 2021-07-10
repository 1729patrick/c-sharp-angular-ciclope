import { Injectable } from "@angular/core";
import { format, parse } from "date-fns";
import { SerieType } from "src/types/ChartData";

export const DATE_FORMAT = "MM/yyyy";

@Injectable({
  providedIn: "root",
})
export class ChartService {
  getSeries = (
    data: { [key: string]: any }[],
    key: string = "divida"
  ): SerieType[] => {
    const dataMapped = data.reduce((prev, { data, ...rest }) => {
      const month = format(new Date(data), DATE_FORMAT);
      prev[month] = (prev[month] || 0) + rest[key];

      return prev;
    }, {});

    return Object.entries(dataMapped).map(([key, value]) => ({
      name: key,
      value,
    }));
  };

  sort = (data: SerieType[]): SerieType[] => {
    return data.sort(
      (a, b) =>
        parse(a.name, DATE_FORMAT, new Date()).getTime() -
        parse(b.name, DATE_FORMAT, new Date()).getTime()
    );
  };

  completeMissingKeys = (
    series1: SerieType[],
    series2: SerieType[]
  ): [SerieType[], SerieType[]] => {
    const keys = [...series1, ...series2].map(({ name }) => name);

    keys.forEach((key) => {
      if (!series1.find(({ name }) => name == key)) {
        series1 = [...series1, { name: key, value: 0 }];
      }
      if (!series2.find(({ name }) => name == key)) {
        series2 = [...series2, { name: key, value: 0 }];
      }
    });

    return [this.sort(series1), this.sort(series2)];
  };
}
