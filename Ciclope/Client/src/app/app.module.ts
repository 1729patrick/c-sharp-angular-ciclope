import { BrowserModule } from "@angular/platform-browser";
import { NgModule, Injector, CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { createCustomElement } from "@angular/elements";

import { AppComponent } from "./app.component";
import { DividaBarComponent } from "./divida-bar/divida-bar.component";
import { DividaLineComponent } from "./divida-line/divida-line.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ResumoComponent } from "./resumo/resumo.component";
import { NgxChartsModule } from "@swimlane/ngx-charts";
import { DividaPizzaComponent } from "./divida-pizza/divida-pizza.component";
import { ApiService } from "./api.service";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatNativeDateModule } from "@angular/material/core";
import { LoadingComponent } from "./loading/loading.component";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";

@NgModule({
  declarations: [
    AppComponent,
    DividaBarComponent,
    DividaLineComponent,
    ResumoComponent,
    DividaPizzaComponent,
    LoadingComponent,
  ],
  imports: [
    BrowserModule,
    NgxChartsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
  ],
  exports: [],
  providers: [ApiService],
  entryComponents: [
    AppComponent,
    DividaBarComponent,
    DividaLineComponent,
    ResumoComponent,
    DividaPizzaComponent,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {
  constructor(private injector: Injector) {}

  ngDoBootstrap() {
    customElements.define(
      "divida-bar",
      createCustomElement(DividaBarComponent, { injector: this.injector })
    );
    customElements.define(
      "divida-line",
      createCustomElement(DividaLineComponent, { injector: this.injector })
    );
    customElements.define(
      "divida-pizza",
      createCustomElement(DividaPizzaComponent, { injector: this.injector })
    );
    customElements.define(
      "financas-resumo",
      createCustomElement(ResumoComponent, { injector: this.injector })
    );
  }
}
