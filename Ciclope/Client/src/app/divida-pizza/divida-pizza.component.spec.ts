import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { DividaPizzaComponent } from "./divida-pizza.component";

describe("DividaPizzaComponent", () => {
  let component: DividaPizzaComponent;
  let fixture: ComponentFixture<DividaPizzaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DividaPizzaComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DividaPizzaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
