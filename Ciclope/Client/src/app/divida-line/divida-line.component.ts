import { Component, Input, OnInit, ViewContainerRef } from "@angular/core";
import { TooltipService } from "@swimlane/ngx-charts";
import { add } from "date-fns";
import { ChartDataType } from "src/types/ChartData";
import { ChartFilterType } from "src/types/ChartFilter";
import { ApiService } from "../api.service";
import { ChartService } from "../chart.service";

@Component({
  selector: "app-divida-line",
  templateUrl: "./divida-line.component.html",
  styleUrls: ["./divida-line.component.css"],
})
export class DividaLineComponent implements OnInit {
  @Input()
  key: string;

  dividas: ChartDataType[];
  view: any[] = [700, 500];

  // options
  legend: boolean = true;
  showLabels: boolean = true;
  animations: boolean = true;
  xAxis: boolean = true;
  yAxis: boolean = true;
  showYAxisLabel: boolean = true;
  showXAxisLabel: boolean = true;
  xAxisLabel: string = "Mês";
  yAxisLabel: string = "Valor";
  timeline: boolean = true;
  legendTitle: string = "Legenda";

  colorScheme = {
    domain: ["#5AA454", "#E44D25", "#CFC0BB", "#7aa3e5", "#a8385d", "#aae3f5"],
  };

  constructor(
    private chartToolTipService: TooltipService,
    private viewContainerRef: ViewContainerRef,
    private apiService: ApiService,
    private chartService: ChartService
  ) {}

  ngOnInit() {
    this.chartToolTipService.injectionService.setRootViewContainer(
      this.viewContainerRef
    );

    this.apiService.setApiKey(this.key);

    this.getResults({
      startDate: add(new Date(), { months: -3 }),
      endDate: new Date(),
    });
  }

  getResults = async ({ startDate, endDate }: ChartFilterType) => {
    const [dividasSS, dividasAT] = await Promise.all([
      this.apiService.getDividaAT({ startDate, endDate }).toPromise(),
      this.apiService.getDividaSS({ startDate, endDate }).toPromise(),
    ]);

    const dividasSSSeries = this.chartService.getSeries(dividasSS);
    const dividasATSeries = this.chartService.getSeries(dividasAT);

    const [dividasATSeries_, dividasSSSeries_] =
      this.chartService.completeMissingKeys(dividasATSeries, dividasSSSeries);

    this.dividas = [
      {
        name: "Dívida AT",
        series: dividasATSeries_,
      },
      {
        name: "Dívida SS",
        series: dividasSSSeries_,
      },
    ];
  };

  dateRangeChange(startDate: HTMLInputElement, endDate: HTMLInputElement) {
    if (startDate.value && endDate.value) {
      this.getResults({
        startDate: new Date(startDate.value),
        endDate: new Date(endDate.value),
      });
    }
  }
}
