import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { DividaLineComponent } from "./divida-line.component";

describe("DividaLineComponent", () => {
  let component: DividaLineComponent;
  let fixture: ComponentFixture<DividaLineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DividaLineComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DividaLineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
