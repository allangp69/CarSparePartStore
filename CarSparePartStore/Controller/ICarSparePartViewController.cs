namespace CarSparePartStore.Controller;

public interface ICarSparePartViewController
{
    CarSparePartViewContent CurrentContent { get; set; }

    void ShowContent(CarSparePartViewContent carSparePartViewContent);
}