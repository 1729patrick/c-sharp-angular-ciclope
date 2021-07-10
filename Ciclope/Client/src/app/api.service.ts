import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { format } from "date-fns";
import { ChartFilterType } from "src/types/ChartFilter";
import { ExpenseATType, ExpenseSSType, CashflowType } from "src/types/API";

export const DATE_FORMAT = "dd-MM-yyyy";

@Injectable()
export class ApiService {
  apiKey: string = "9c10dfe8-2f92-4af6-a63d-23bcac2945bb"; //api key for debugging
  constructor(private http: HttpClient) {}

  setApiKey(key: string = this.apiKey) {
    this.apiKey = key;
  }

  getFluxoCaixa(args: ChartFilterType): Observable<CashflowType[]> {
    const startDate = format(args.startDate, DATE_FORMAT);
    const endDate = format(args.endDate, DATE_FORMAT);

    return this.http.get<CashflowType[]>(`/api/Cashflow/${this.apiKey}`, {
      params: { startDate, endDate },
    });
  }

  getDividaAT(args: ChartFilterType): Observable<ExpenseATType[]> {
    const startDate = format(args.startDate, DATE_FORMAT);
    const endDate = format(args.endDate, DATE_FORMAT);

    return this.http.get<ExpenseATType[]>(`/api/DividaAT/${this.apiKey}`, {
      params: { startDate, endDate },
    });
  }

  getDividaSS(args: ChartFilterType): Observable<ExpenseSSType[]> {
    const startDate = format(args.startDate, DATE_FORMAT);
    const endDate = format(args.endDate, DATE_FORMAT);

    return this.http.get<ExpenseSSType[]>(`/api/DividaSS/${this.apiKey}`, {
      params: { startDate, endDate },
    });
  }

  getDivida(
    type: "DividaAT" | "DividaSS",
    args: ChartFilterType
  ): Observable<any[]> {
    const startDate = format(args.startDate, DATE_FORMAT);
    const endDate = format(args.endDate, DATE_FORMAT);

    return this.http.get<any[]>(`/api/${type}/${this.apiKey}`, {
      params: { startDate, endDate },
    });
  }
}
