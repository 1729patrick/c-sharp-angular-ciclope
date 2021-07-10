import { Component, Input, OnInit, ViewContainerRef } from "@angular/core";
import { TooltipService } from "@swimlane/ngx-charts";
import { add } from "date-fns";
import { SerieType } from "src/types/ChartData";
import { ChartFilterType } from "src/types/ChartFilter";
import { ApiService } from "../api.service";
import { ChartService } from "../chart.service";

@Component({
  selector: "app-divida-pizza",
  templateUrl: "./divida-pizza.component.html",
  styleUrls: ["./divida-pizza.component.css"],
})
export class DividaPizzaComponent implements OnInit {
  @Input()
  key: string;

  dividas: SerieType[];
  view: any[] = [700, 400];

  // options
  gradient: boolean = true;
  showLegend: boolean = true;
  showLabels: boolean = true;
  isDoughnut: boolean = false;
  legendPosition: string = "below";
  legendTitle: string = "Legenda";

  colorScheme = {
    domain: ["#5AA454", "#A10A28", "#C7B42C", "#AAAAAA"],
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
    const [dividasSS, dividasAT, fluxosCaixa] = await Promise.all([
      this.apiService.getDividaAT({ startDate, endDate }).toPromise(),
      this.apiService.getDividaSS({ startDate, endDate }).toPromise(),
      this.apiService.getFluxoCaixa({ startDate, endDate }).toPromise(),
    ]);

    const dividasSSSeries = this.chartService.getSeries(dividasSS);
    const dividasATSeries = this.chartService.getSeries(dividasAT);
    const fluxosCaixaSeries = this.chartService.getSeries(fluxosCaixa, "valor");

    this.dividas = [
      {
        name: "Dívida AT",
        value: dividasATSeries.reduce((prev, curr) => prev + curr.value, 0),
      },
      {
        name: "Dívida SS",
        value: dividasSSSeries.reduce((prev, curr) => prev + curr.value, 0),
      },
      {
        name: "Fluxo de Caixa",
        value: fluxosCaixaSeries.reduce((prev, curr) => prev + curr.value, 0),
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
