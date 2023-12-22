using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class ServiceList : Service
    {
        private List<Service> _serviceBuildings;
        public List<Service> ServiceBuildings { get { return _serviceBuildings; } }
        public ServiceList()
        {
            _serviceBuildings = new List<Service>();
        }

        #region Methods
        public void Add(Service service)
        {
            _serviceBuildings.Add(service);
        }
        public Service? getServiceByField(Field field) {
            foreach (Service service in _serviceBuildings)
            {
                if (service.Fields!.Contains(field))
                {
                    return service;
                }
            }
            return null;
        }
        public Service? GetServiceBuilding(String name)
        {
            foreach (Service service in _serviceBuildings)
            {
                switch (service.Name)
                {
                    case "Police":
                        if (name == "Police")
                        {
                            return (PoliceDepartment)service;
                        }
                        break;
                    case "Stadium":
                        if (name == "Stadium")
                        {
                            return (Stadium)service;
                        }
                        break;
                    case "PowerPlant":
                        if (name == "PowerPlant")
                        {
                            return (PowerPlant)service;
                        }
                        break;
                    case "University":
                        if (name == "University")
                        {
                            return (University)service;
                        }
                        break;
                    case "School":
                        if (name == "School")
                        {
                            return (School)service;
                        }
                        break;
                    default:
                        break;
                }
            }
            return null;
        }
        public int Deconstruct(Service? service)
        {
            _serviceBuildings.Remove(service!);
            return BuildPrice / 2;
        }
        #endregion
    }
}


