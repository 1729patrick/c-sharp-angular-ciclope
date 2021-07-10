import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { DividaBarComponent } from "./divida-bar.component";

describe("DividaBarComponent", () => {
  let component: DividaBarComponent;
  let fixture: ComponentFixture<DividaBarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DividaBarComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DividaBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
