import { Component, Input, OnInit, ViewContainerRef } from "@angular/core";
import { TooltipService } from "@swimlane/ngx-charts";
import { add } from "date-fns";
import { ApiService } from "../api.service";
import { ChartFilterType } from "src/types/ChartFilter";
import { ChartService } from "../chart.service";
import { SerieType } from "src/types/ChartData";

@Component({
  selector: "app-divida-bar",
  templateUrl: "./divida-bar.component.html",
  styleUrls: ["./divida-bar.component.css"],
})
export class DividaBarComponent implements OnInit {
  @Input()
  type: "DividaAT" | "DividaSS";

  @Input()
  key: string;

  dividas: SerieType[];

  view: any[] = [700, 400];

  // options
  showXAxis: boolean = true;
  showYAxis: boolean = true;
  gradient: boolean = false;
  showLegend: boolean = true;
  showXAxisLabel: boolean = true;
  xAxisLabel: string = "Mês";
  showYAxisLabel: boolean = true;
  yAxisLabel: string = "Valor";
  title: string = "";
  legendTitle: string = "Legenda";

  configs = {
    DividaSS: {
      title: "Dívida Segurança Social",
    },
    DividaAT: {
      title: "Dívida Atividade Tributária",
    },
  };

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

    this.title = this.configs[this.type]?.title;

    this.apiService.setApiKey(this.key);

    this.getResults({
      startDate: add(new Date(), { months: -3 }),
      endDate: new Date(),
    });
  }

  getResults = ({ startDate, endDate }: ChartFilterType) => {
    this.apiService
      .getDivida(this.type, { startDate, endDate })
      .subscribe((dividas) => {
        this.dividas = this.chartService.getSeries(dividas);
      });
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
